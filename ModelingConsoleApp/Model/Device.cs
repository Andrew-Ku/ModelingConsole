using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelingConsoleApp.Const;
using ModelingConsoleApp.Infrastructure;

namespace ModelingConsoleApp.Model
{
    /// <summary>
    /// Устройство
    /// </summary>
    public class Device
    {
        public Device(string name)
        {
            Name = name;
            TaskQueue = new List<TaskBase>();
            Channel1 = new Channel();
            Channel2 = new Channel();
        }

        public  Channel Channel1 { get; set; }
        public  Channel Channel2 { get; set; }
        public string Name { get; set; }
        public List<TaskBase> TaskQueue { get; set; }

        public double TaskGenTimeSum { get; set; }
        public int TaskReleaseCount { get; set; }
        public int TaskWeightSum { get; set; }
        public double QueueLengthSum { get; set; }
        public double QueueTimeSum { get; set; }
        public double QueueWeightTimeSum { get; set; }

        /// <summary>
        /// Освободить заданный канал
        /// </summary>
        public void Release(int channelNum = 12)
        {
            try
            {
                switch (channelNum)
                {
                    case Channels.Channel1:
                        TaskGenTimeSum += Program.ModelingTime - Channel1.CurrentTask.GenerateTime;
                        QueueTimeSum += Channel1.CurrentTask.PopQueueTime - Channel1.CurrentTask.PushQueueTime;
                        QueueWeightTimeSum += (Channel1.CurrentTask.PopQueueTime - Channel1.CurrentTask.PushQueueTime) * Channel1.CurrentTask.Weight;
                        Channel1.CurrentTask = null;
                        Channel1.IsAvailable = true;
                        break;
                    case Channels.Channel2:
                        TaskGenTimeSum += Program.ModelingTime - Channel2.CurrentTask.GenerateTime;
                        QueueTimeSum += Channel2.CurrentTask.PopQueueTime - Channel2.CurrentTask.PushQueueTime;
                        QueueWeightTimeSum += (Channel2.CurrentTask.PopQueueTime - Channel2.CurrentTask.PushQueueTime) * Channel2.CurrentTask.Weight;
                        Channel2.CurrentTask = null;
                        Channel2.IsAvailable = true;
                        break;
                    case Channels.AllChannels:
                        TaskGenTimeSum += Program.ModelingTime - Channel1.CurrentTask.GenerateTime;
                        QueueTimeSum += Channel1.CurrentTask.PopQueueTime - Channel1.CurrentTask.PushQueueTime;
                        QueueWeightTimeSum += (Channel1.CurrentTask.PopQueueTime - Channel1.CurrentTask.PushQueueTime) * Channel1.CurrentTask.Weight;
                        Channel1.CurrentTask = null;
                        Channel1.IsAvailable = true;
                        Channel2.CurrentTask = null;
                        Channel2.IsAvailable = true;
                        break;
                    default:
                        throw new ArgumentException("Недопустимый номер канала для устройства" + Name);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка с каналами. Канал 1 - {0}, Канал 2 - {1}. Пытается осободить канал {2} ",Channel1.IsAvailable);
            }

            TaskReleaseCount++; // Счетчик прошедших через систему задач
        }

        /// <summary>
        /// Занять канал задачей
        /// </summary>
        /// <param name="task"></param>
        /// <param name="channelNum"></param>
        public void Seize(TaskBase task, int channelNum)
        {
            switch (channelNum)
            {
                case Channels.Channel1:
                    Channel1.CurrentTask = task;
                    Channel1.IsAvailable = false;
                    break;
                case Channels.Channel2:
                    Channel2.CurrentTask = task;
                    Channel2.IsAvailable = false;
                    break;
                case Channels.AllChannels:
                    if(task.Type!=TaskTypes.ClassC)
                        throw new ArgumentException("Задача не может занимать оба канала одновременно" + Name);
                    Channel1.CurrentTask = task;
                    Channel1.IsAvailable = false;
                    Channel2.CurrentTask = task;
                    Channel2.IsAvailable = false;
                    break;
                default: throw new ArgumentException("Недопустимый номер канала для устройства" + Name);
            }
        }

        /// <summary>
        /// Возвращает состояние канала True - свободен False - занят
        /// </summary>
        public bool IsChannelAvailable(int channelNum)
        {
            switch (channelNum)
            {
                case Channels.Channel1:
                    return Channel1.IsAvailable;
                case Channels.Channel2:
                    return Channel2.IsAvailable;
               default: throw new ArgumentException("Недопустимый номер канала для устройства" + Name);
            }
        }
        /// <summary>
        /// Среднее время прохождения задачи через систему
        /// </summary>
        public double TasksAverageSystemTime
        {
            get { return TaskGenTimeSum / TaskReleaseCount; }
        }
        /// <summary>
        /// Средняя длина очереди
        /// </summary>
        public double QueueLengthAverage
        {
            get { return QueueLengthSum / TaskBase.GenCount; }
        }
        /// <summary>
        /// Среднее время ожидания в очереди
        /// </summary>
        public double QueueTimeAverage
        {
            get { return QueueTimeSum / TaskBase.OutQueueCount; }
        }
        /// <summary>
        /// Средневзвешенное время ожидания в очереди 
        /// </summary>
        public double QueueWeightTimeAverage
        {
            get { return QueueWeightTimeSum / TaskBase.OutQueueWeightCount; }
        }
    }
}
