using ERPSystem_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem_Services.Interfaces
{
    public interface IQuestionService
    {
        void AddInterviewQuestions(int content_id, List<InterviewQuestionModel> interviewQuestions);
        List<InterviewQuestionModel> GetAllInterviewQuestions();
        List<InterviewQuestionModel> GetTopicWiseInterviewQuestions(int topic_id);
        List<InterviewQuestionModel> GetContentWiseInterviewQuestions(int content_id);
        void AddProgramQuestion(ProgramQuestionModel program);
        void UpdateProgramQuestion(ProgramQuestionModel program);
        void DeleteProgramQuestion(int program_question_id);
        void RestoreProgramQuestion(int program_question_id);
        List<ProgramQuestionModel> GetAllProgramQuestions();
        List<ProgramQuestionModel> GetTopicWiseProgramQuestions(int topic_id);
        List<ProgramQuestionModel> GetContentWiseProgramQuestions(int content_id);
        void AddProgramAnswer(ProgramAnswerModel answer);
        void UpdateProgramAnswer(ProgramAnswerModel answer);
        void DeleteProgramAnswer(int program_answer_id);
        void RestoreProgramAnswer(int program_id);
        List<ProgramAnswerModel> GetAllProgramAnswers();
        List<ProgramAnswerModel> GetTopicWiseProgramAnswers(int topic_id);
        List<ProgramAnswerModel> GetContentWiseProgramAnswers(int content_id);
    }
}
