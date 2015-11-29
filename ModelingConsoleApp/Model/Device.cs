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

        /// <summary>
        /// Освободить заданный канал
        /// </summary>
        public void Release(int channelNum = 12)
        {
            switch (channelNum)
            {
                case Channels.Channel1:
                    TaskGenTimeSum += Program.ModelingTime - Channel1.CurrentTask.GenerateTime;
                    QueueTimeSum += Channel1.CurrentTask.PopQueueTime - Channel1.CurrentTask.PushQueueTime;
                    Channel1.CurrentTask = null;
                    Channel1.IsAvailable = true;
                    break;
                case Channels.Channel2:
                    TaskGenTimeSum += Program.ModelingTime - Channel2.CurrentTask.GenerateTime;
                    QueueTimeSum += Channel2.CurrentTask.PopQueueTime - Channel2.CurrentTask.PushQueueTime;
                    Channel2.CurrentTask = null;
                    Channel2.IsAvailable = true;
                    break;
                case Channels.AllChannels:
                    TaskGenTimeSum += Program.ModelingTime - Channel1.CurrentTask.GenerateTime;
                    QueueTimeSum += Channel1.CurrentTask.PopQueueTime - Channel1.CurrentTask.PushQueueTime;
                    Channel1.CurrentTask = null;
                    Channel1.IsAvailable = true;
                    Channel2.CurrentTask = null;
                    Channel2.IsAvailable = true;
                    break;
                default:
                    throw new ArgumentException("Недопустимый номер канала для устройства" + Name);
            }

            TaskReleaseCount++; // Счетчик прошедших через систему задач
        }

        // Занять канал
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

        public double TasksAverageSystemTime
        {
            get { return TaskGenTimeSum / TaskReleaseCount; }
        }

        public double QueueLengthAverage
        {
            get { return QueueLengthSum / TaskBase.GenCount; }
        }

        public double QueueTimeAverage
        {
            get { return QueueTimeSum / TaskBase.OutQueueCount; }
        }
    }
}
