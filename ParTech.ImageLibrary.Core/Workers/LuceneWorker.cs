using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Castle.Core.Logging;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using ParTech.ImageLibrary.Core.Interfaces;
using ParTech.ImageLibrary.Core.Models;
using ParTech.ImageLibrary.Core.Utils;
using Version = Lucene.Net.Util.Version;

namespace ParTech.ImageLibrary.Core.Workers
{
    public interface ILuceneWorker : IWorker
    {
        void AddUpdateLuceneIndex(string language, Product product);

        void AddUpdateLuceneIndex(string language, IEnumerable<Product> products);

        bool ClearLuceneIndex();

        void ClearLuceneIndexRecord(int recordid);

        IEnumerable<Product> GetAllIndexRecords();
        
        void Optimize();

        IEnumerable<Product> Search(string input, string fieldName);

        IEnumerable<Product> SearchDefault(string input, string fieldName);
    }

    public class LuceneWorker : ILuceneWorker
    {
        #region Properties

        private FSDirectory _directoryTemp;

        private FSDirectory Directory
        {
            get
            {
                if (_directoryTemp == null)
                {
                    _directoryTemp = FSDirectory.Open(new DirectoryInfo(LuceneDir));
                }

                if (IndexWriter.IsLocked(_directoryTemp))
                {
                    IndexWriter.Unlock(_directoryTemp);
                }

                var lockFilePath = Path.Combine(LuceneDir, "write.lock");

                if (File.Exists(lockFilePath))
                {
                    File.Delete(lockFilePath);
                }

                return _directoryTemp;
            }
        }

        public string DefaultSortField
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("Lucene.DefaultSortField");
            }
        }

        public ILogger Logger { get; set; }

        public string LuceneDir = ConfigurationManager.AppSettings.Get("Lucene.IndexDirectory");

        public int MaxNoOfHits = 0;

        #endregion

        public LuceneWorker()
        {
            MaxNoOfHits = int.Parse(ConfigurationManager.AppSettings.Get("Lucene.MaxNoOfHits"));
        }

        public void AddUpdateLuceneIndex(string language, Product product)
        {
            AddUpdateLuceneIndex(language, new List<Product> { product });
        }

        public void AddUpdateLuceneIndex(string language, IEnumerable<Product> products)
        {
            // init lucene
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // add data to lucene search index (replaces older entries if any)
                foreach (var product in products)
                {
                    _addToLuceneIndex(language, product, writer);
                }

                // close handles
                analyzer.Close();
                writer.Dispose();
            }
        }

        public bool ClearLuceneIndex()
        {
            try
            {
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);
                using (var writer = new IndexWriter(Directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    // remove older index entries
                    writer.DeleteAll();

                    // close handles
                    analyzer.Close();
                    writer.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat(" ClearLuceneIndex - error [{0}] - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);

                return false;
            }
            return true;
        }

        public void ClearLuceneIndexRecord(int recordid)
        {
            // init lucene
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // remove older index entry
                var searchQuery = new TermQuery(new Term("Id", recordid.ToString(CultureInfo.InvariantCulture)));
                writer.DeleteDocuments(searchQuery);

                // close handles
                analyzer.Close();
                writer.Dispose();
            }
        }

        public IEnumerable<Product> GetAllIndexRecords()
        {
            // validate search index
            if (!System.IO.Directory.EnumerateFiles(LuceneDir).Any())
            {
                return new List<Product>();
            }

            // set up lucene searcher
            var searcher = new IndexSearcher(Directory, false);
            var reader = IndexReader.Open(Directory, false);
            var docs = new List<Document>();
            var term = reader.TermDocs();
            // v 2.9.4: use 'term.Doc()'
            // v 3.0.3: use 'term.Doc'
            while (term.Next())
            {
                docs.Add(searcher.Doc(term.Doc));
            }
            reader.Dispose();
            searcher.Dispose();

            return MapLuceneToDataList(docs);
        }

        public void Optimize()
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                analyzer.Close();
                writer.Optimize();
                writer.Dispose();
            }
        }

        public IEnumerable<Product> Search(string input, string fieldName = "")
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            var terms = input.Trim()
                             .Replace("-", " ")
                             .Split(' ')
                             .Where(x => !string.IsNullOrEmpty(x))
                             .Select(x => x.Trim() + "*");
            input = string.Join(" ", terms);

            return MainSearch(input, fieldName);
        }

        public IEnumerable<Product> SearchDefault(string input, string fieldName = "")
        {
            return string.IsNullOrEmpty(input) ? null : MainSearch(input, fieldName);
        }

        #region Private methods

        private static void _addToLuceneIndex(string language, Product entireProduct, IndexWriter writer)
        {
            // remove older index entry
            var searchQuery = new TermQuery(new Term("Id", entireProduct.ProductID.ToString(CultureInfo.InvariantCulture)));
            writer.DeleteDocuments(searchQuery);

            // add new index entry
            var doc = new Document();

            var allContent = entireProduct.Name + " " + entireProduct.EDI + " " + entireProduct.SKU 
                + " " + entireProduct.Year.ToString(CultureInfo.InvariantCulture) + " " + entireProduct.Material 
                + " " + entireProduct.Size;

            // add lucene fields mapped to db fields
            doc.Add(new Field("Id", entireProduct.ProductID.ToString(CultureInfo.InvariantCulture), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("Language", language, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("Name", entireProduct.Name, Field.Store.YES, Field.Index.ANALYZED));
            
            if (!string.IsNullOrEmpty(entireProduct.EDI))
            {
                doc.Add(new Field("Edi", entireProduct.EDI, Field.Store.YES, Field.Index.ANALYZED));
            }

            if (!string.IsNullOrEmpty(entireProduct.SKU))
            {
                doc.Add(new Field("Sku", entireProduct.SKU, Field.Store.YES, Field.Index.ANALYZED));
            }

            doc.Add(new Field("Year", entireProduct.Year.ToString(CultureInfo.InvariantCulture), Field.Store.YES, Field.Index.NOT_ANALYZED));

            if (!string.IsNullOrEmpty(entireProduct.Material))
            {
                doc.Add(new Field("Material", entireProduct.Material, Field.Store.YES, Field.Index.ANALYZED));
            }

            if (!string.IsNullOrEmpty(entireProduct.Size))
            {
                doc.Add(new Field("Size", entireProduct.Size, Field.Store.YES, Field.Index.ANALYZED));
            }

            if (entireProduct.Season != null)
            {
                allContent += " " + LanguageString.GetStringForCurrentLanguage(language, entireProduct.Season.Name);
                doc.Add(new Field("SeasonId", entireProduct.SeasonID.ToString(CultureInfo.InvariantCulture), Field.Store.YES, Field.Index.ANALYZED));
            }

            if (entireProduct.Gender != null)
            {
                allContent += " " + LanguageString.GetStringForCurrentLanguage(language, entireProduct.Gender.Name);
                doc.Add(new Field("GenderId", entireProduct.GenderID.ToString(CultureInfo.InvariantCulture), Field.Store.YES, Field.Index.ANALYZED));
            }

            if (entireProduct.Category != null)
            {
                allContent += " " + LanguageString.GetStringForCurrentLanguage(language, entireProduct.Category.Name);
                doc.Add(new Field("CategoryId", entireProduct.CategoryID.ToString(CultureInfo.InvariantCulture), Field.Store.YES, Field.Index.ANALYZED));
            }

            if (entireProduct.Collection != null)
            {
                allContent += " " + entireProduct.Collection.Name;
                doc.Add(new Field("CollectionId", entireProduct.CollectionID.ToString(), Field.Store.YES, Field.Index.ANALYZED));
            }

            if (entireProduct.Brand != null)
            {
                allContent += " " + entireProduct.Brand.Name;
                doc.Add(new Field("BrandId", entireProduct.CollectionID.ToString(), Field.Store.YES, Field.Index.ANALYZED));
            }

            doc.Add(new Field("Created", entireProduct.created.ToString("yyyyMMdd"), Field.Store.YES, Field.Index.NOT_ANALYZED));

            // add lucene field for all fields
            doc.Add(new Field("AllContent", allContent, Field.Store.YES, Field.Index.ANALYZED));

            // add entry to index
            writer.AddDocument(doc);
        }

        private IEnumerable<Product> MainSearch(string searchTerms, string searchField = "")
        {
            // validation
            if (string.IsNullOrEmpty(searchTerms.Replace("*", "").Replace("?", "")))
            {
                return null;
            }

            // '*' or '?' not allowed as first character in WildcardQuery
            var firstCharacter = searchTerms.Substring(0, 1);
            if (firstCharacter == "*" || firstCharacter == "?")
            {
                searchTerms = searchTerms.Substring(1, searchTerms.Length - 1);
            }

            // set up lucene searcher
            using (var searcher = new IndexSearcher(Directory, false))
            {
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);

                // search by single field
                if (!string.IsNullOrEmpty(searchField))
                {
                    var mainQuery = new BooleanQuery();

                    var languageParser = new QueryParser(Version.LUCENE_30, "Language", analyzer);
                    var languageQuery = languageParser.Parse(Thread.CurrentThread.CurrentCulture.ThreeLetterISOLanguageName);

                    mainQuery.Add(new BooleanClause(languageQuery, Occur.MUST));

                    var searchParser = new QueryParser(Version.LUCENE_30, searchField, analyzer);
                    var searchQuery = searchParser.Parse(searchTerms);

                    mainQuery.Add(new BooleanClause(searchQuery, Occur.MUST));

                    var hits = searcher.Search(mainQuery, MaxNoOfHits).ScoreDocs;
                    var results = MapLuceneToDataList(hits, searcher);

                    analyzer.Close();
                    searcher.Dispose();
                    return results;
                }
                // search by multiple fields (ordered by RELEVANCE)
                else
                {
                    var mainQuery = new BooleanQuery();

                    var languageParser = new QueryParser(Version.LUCENE_30, "Language", analyzer);
                    var languageQuery = languageParser.Parse(Thread.CurrentThread.CurrentCulture.ThreeLetterISOLanguageName);

                    mainQuery.Add(new BooleanClause(languageQuery, Occur.MUST));

                    var searchParser = new MultiFieldQueryParser(Version.LUCENE_30, new[] { "AllContent" }, analyzer);
                    var searchQuery = searchParser.Parse(searchTerms);

                    mainQuery.Add(new BooleanClause(searchQuery, Occur.MUST));

                    var hits = searcher.Search(mainQuery, null, MaxNoOfHits, 
                        new Sort(new SortField(DefaultSortField, SortField.STRING))).ScoreDocs;
                    var results = MapLuceneToDataList(hits, searcher);
                    analyzer.Close();
                    searcher.Dispose();
                    return results;
                }
            }
        }

        private IEnumerable<Product> MapLuceneToDataList(IEnumerable<Document> hits)
        {
            return hits.Select(MapLuceneDocumentToData).ToList();
        }

        private IEnumerable<Product> MapLuceneToDataList(IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
        {
            // v 2.9.4: use 'hit.doc'
            // v 3.0.3: use 'hit.Doc'
            return hits.Select(hit => MapLuceneDocumentToData(searcher.Doc(hit.Doc))).ToList();
        }

        private Product MapLuceneDocumentToData(Document doc)
        {
            var productId = Convert.ToInt32(doc.Get("Id"));
            
            return new Product
            {
                ProductID = productId,
                Name = doc.Get("Name"),
                EDI = doc.Get("Edi"),
                SKU = doc.Get("Sku"),
                Year = int.Parse(doc.Get("Year")),
                Material = doc.Get("Material"),
                Size = doc.Get("Size"),
            };
        }

        #endregion
    }
}
