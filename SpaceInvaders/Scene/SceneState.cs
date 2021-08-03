
using System.Diagnostics;

namespace SpaceInvaders
{
    public abstract class SceneState
    {
        public abstract void Handle();
        public abstract void Initialize();
        public abstract void Update(float systemTime);
        public abstract void Draw();
        public abstract void Transition();

        public virtual void InputLeft()
        {

        }

        public virtual void InputRight()
        {

        }

        public virtual void InputSpace()
        {

        }

        public virtual void InputK()
        {

        }

        public virtual void Input1()
        {

        }

        public virtual void Input2()
        {

        }

    }
}
