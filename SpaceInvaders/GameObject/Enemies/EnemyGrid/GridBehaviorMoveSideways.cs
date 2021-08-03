using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class GridBehaviorMoveSideways : GridBehavior
    {
        public GridBehaviorMoveSideways(EnemyGrid pGrid, float deltaX)
            : base(pGrid)
        {
            this.deltaX = deltaX;
        }

        public override void Advance()
        {
            if (!pGrid.IsCompositeEmpty())
            {

                ForwardIterator pFor = new ForwardIterator(pGrid);
                bool somethingMoved = false;

                Component pNode = pFor.First();
                while (!pFor.IsDone())
                {
                    GameObject pGameObj = (GameObject)pNode;
                    pGameObj.x += this.deltaX;

                    somethingMoved = true;
                    pNode = pFor.Next();
                }

                if (somethingMoved)
                {
                    pGrid.PlayMovementEffects();
                }

                //TimerMan.Add(TimeEvent.Name.GridMovement, new GridMovement(pGrid), pGrid.GetGridSpeed()); // we need to start this up again
                //pGrid.QueueGridMovement();
            }
        }

        public override void Handle()
        {
            pGrid.SetGridBehavior(EnemyGrid.State.MoveDown);
        }

        public void UpdateDeltaX(float deltaX)
        {
            this.deltaX = deltaX;
        }

        float deltaX;
    }
}
