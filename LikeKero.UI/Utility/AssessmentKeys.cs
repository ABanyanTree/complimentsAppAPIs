using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LikeKero.UI.Utility
{
    public class AssessmentKeys
    {
        public enum TestStatus
        {
            Completed,
            Started
        }
        public enum PassFailStatus
        {
            Pass,
            Fail
        }
        public enum QuestionType
        {
            MATCH = 0,
            MRQ = 1,
        }

        public const string SESSION_TEST_ELAPSED_TIME = "TestElaspedTime";
        public const string SESSION_TESTELAPSEDTIME = "TestElaspedTime";
    }
}
