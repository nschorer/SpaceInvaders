using System;
using System.Diagnostics;

namespace SpaceInvaders {
    class GridBehaviorMoveDown : GridBehavior
    {
        public GridBehaviorMoveDown(EnemyGrid pGrid, float deltaY)
            : base(pGrid)
        {
            this.deltaY = deltaY;
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
                    pGameObj.y -= deltaY;

                    somethingMoved = true;
                    pNode = pFor.Next();
                }

                if (somethingMoved)
                {
                    pGrid.PlayMovementEffects();
                }

                // We only go down once at a time
                Handle();

                //TimerMan.Add(TimeEvent.Name.GridMovement, new GridMovement(pGrid), pGrid.GetGridSpeed()); // we need to start this up again
                //pGrid.QueueGridMovement();
            }
        }


        public override void Handle()
        {
            pGrid.ToggleGridDirection();
            pGrid.SetGridBehavior(EnemyGrid.State.MoveTurnAround);
        }

        float deltaY;
    }
}
