using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelingConsoleApp.Const;

namespace ModelingConsoleApp.Model
{
    public class Device
    {
        public Device(string name)
        {
            Name = name;
            TaskQueue = new TaskQueue<TaskBase>();
            IsAvailableChannel1 = true;
            IsAvailableChannel2 = true;
        }


      //  private _channel

        public string Name { get; set; }
        //public TaskBase Channel1
        //{
        //    get { return Channel1; }
        //    set
        //    {
        //        Channel1 = value;
        //        IsAvailableChannel1 = value == null;
        //    }
        //}
        //public TaskBase Channel2
        //{
        //    get { return Channel2; }
        //    set
        //    {
        //        Channel2 = value;
        //        IsAvailableChannel2 = value == null;
        //    }
        //}
        public DeviceInfo DeviceInfo { get; set; }

        // Очередь к устройству
        public TaskQueue<TaskBase> TaskQueue { get; set; }

        // Канал 1 свободен
        public bool IsAvailableChannel1 { get; set; }

        // Канал 2 свободен
        public bool IsAvailableChannel2 { get; set; }

        // Доступно ли устройство
        public bool IsDeviceAvailable { get; set; }

        #region События
        ///
        /// 
        /// 
        /// 
        ///
        #endregion



        // Освободить каналы
        //public void Release()
        //{
        //    Channel1 = null;
        //    Channel2 = null;
        //}

        // Освободить заданный канал
        //public void Release(int channelNum)
        //{
        //    switch (channelNum)
        //    {
        //        case 1:
        //            Channel1 = null;
        //            break;
        //        case 2:
        //            Channel2 = null;
        //            break;
        //        default: throw new ArgumentException("Недопустимый номер канала для устройства" + Name);
        //    }
        //}

        // Занять устройство
        //public void Seize(TaskBase task)
        //{
        //    if (task.Type == TaskTypes.ClassA || task.Type == TaskTypes.ClassB)
        //    {
        //        if (IsAvailableChannel1)
        //        {
        //            Channel1 = task;
        //            IsAvailableChannel1 = false;
        //        }
        //        else if (IsAvailableChannel2)
        //        {
        //            Channel2 = task;
        //            IsAvailableChannel2 = false;
        //        }
        //        else
        //        {
        //            TaskQueue.Enqueue(task);
        //        }
        //    }
        //    // Задача класса C занимает 2 канала
        //    else
        //    {
        //        if (IsAvailableChannel1 && IsAvailableChannel2)
        //        {
        //            Channel1 = task;
        //            Channel2 = task;
        //            IsAvailableChannel1 = false;
        //            IsAvailableChannel2 = false;
        //        }
        //        else
        //        {
        //            TaskQueue.Enqueue(task);
        //        }
        //    }
       // }

    }
}
