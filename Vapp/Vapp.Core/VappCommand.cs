using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.Core
{
    public class VappCommand
    {
        #region Constructor

        public VappCommand(Action<object> action, uint param = 0)
        {
            this.DelegateAction = action;

            if (param > 8)
                throw new ArgumentException();

            this.Parameters = param;
        }

        #endregion

        #region Properties

        private Action<object> DelegateAction { get; set; }

        public uint Parameters { get; set; }

        public bool CanInvoke
        {
            get { return this.DelegateAction != null; }
        }

        #endregion

        #region Method

        public void Invoke(params object[] objects)
        {
            switch (this.Parameters)
            {
                case 0:
                    this.DelegateAction.Invoke(null);
                    break;
                case 1:
                    this.DelegateAction.Invoke(objects[0]);
                    break;
                case 2:
                    this.DelegateAction.Invoke(Tuple.Create(objects[0], objects[1]));
                    break;
                case 3:
                    this.DelegateAction.Invoke(Tuple.Create(objects[0], objects[1], objects[2]));
                    break;
                case 4:
                    this.DelegateAction.Invoke(Tuple.Create(objects[0], objects[1], objects[2], objects[3]));
                    break;
                case 5:
                    this.DelegateAction.Invoke(Tuple.Create(objects[0], objects[1], objects[2], objects[3], objects[4]));
                    break;
                case 6:
                    this.DelegateAction.Invoke(Tuple.Create(objects[0], objects[1], objects[2], objects[3], objects[4], objects[5]));
                    break;
                case 7:
                    this.DelegateAction.Invoke(Tuple.Create(objects[0], objects[1], objects[2], objects[3], objects[4], objects[5], objects[6]));
                    break;
                case 8:
                    this.DelegateAction.Invoke(Tuple.Create(objects[0], objects[1], objects[2], objects[3], objects[4], objects[5], objects[6], objects[7]));
                    break;
            }
        }

        #endregion
    }
}
