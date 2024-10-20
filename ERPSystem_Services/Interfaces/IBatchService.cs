 
using ERPSystem_Models;

namespace ERPSystem_Services.Interfaces
{
    public interface IBatchService
    {
        void AddBatch(BatchModel batch);
        void AddBatchStudent(BatchStudentModel student);
        void AddBatchSchedule(BatchScheduleModel schedule);
        List<BatchModel> GetAllBatches();
        List<TrainerModel> GetAllTrainers();
      
        List<BatchStudentModel> GetAllBatchStudents();
        List<BatchStudentModel> GetStudentWiseBatches(int student_id);
        List<BatchScheduleModel> GetAllBatchSchedules();

        BatchModel GetBatch(int id);
        List<BatchStudentModel> GetBatchWiseStudents(int batch_id);
        List<TopicStudentModel> GetTopicWiseStudents(int batch_id);
        List<BatchScheduleModel> GetBatchWiseSchedule(int batch_id);
        List<BatchModel> GetTrainerWiseBatches(int trainer_id);
        BatchScheduleModel GetScheduleWiseSchedule(int batch_schedule_id);

        void MarkStudentScheduleAttendance(ScheduleAttendanceModel s);
        List<StudentMarkAttendance> GetBatchWiseStudentAttendance(int batch_id, int registration_id);
    }
}
