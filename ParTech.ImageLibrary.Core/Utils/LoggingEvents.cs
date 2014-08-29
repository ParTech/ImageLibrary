using System;
using ParTech.ImageLibrary.Core.Models;
using ParTech.ImageLibrary.Core.Repositories;

namespace ParTech.ImageLibrary.Core.Utils
{
    public class LoggingEvents
    {
        public static LoggedEvent LogStartEvent(IObjectRepository objectRepository, string eventName, string eventDetails)
        {
            var newLoggedEvent = new LoggedEvent
            {
                DateStarted = DateTime.Now,
                Name = eventName
            };

            if (!string.IsNullOrEmpty(eventDetails))
            {
                newLoggedEvent.Details = eventDetails;
            }

            newLoggedEvent = objectRepository.SaveLoggedEvent(newLoggedEvent);

            return newLoggedEvent;
        }

        public static void LogEndEvent(IObjectRepository objectRepository, int loggedEventId, string eventDetails)
        {
            var loggedEvent = objectRepository.GetLoggedEvent(loggedEventId);
            if (loggedEvent != null)
            {
                loggedEvent.DateEnded = DateTime.Now;

                if (!string.IsNullOrEmpty(loggedEvent.Details) 
                    && !string.IsNullOrEmpty(eventDetails))
                {
                    loggedEvent.Details = string.Concat(loggedEvent.Details, " ", eventDetails);
                }
                else if (!string.IsNullOrEmpty(eventDetails))
                {
                    loggedEvent.Details = eventDetails;
                }
                
                objectRepository.SaveLoggedEvent(loggedEvent);
            }
        }
    }
}
