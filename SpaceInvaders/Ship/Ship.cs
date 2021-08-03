using System;
using System.Diagnostics;


namespace SpaceInvaders
{
    public class Ship : ShipCategory
    {

           public Ship(GameObject.Name name, GameSprite.Name spriteName, float posX, float posY, ShipMan pShipManager)
            : base(name, spriteName, ShipCategory.Type.Ship)
        {
            this.x = posX;
            this.y = posY;

            this.shipSpeed = 3.0f;
            this.missileState = null;
            this.movementState = null;
            this.pShipMan = pShipManager;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void VisitBomb(Bomb b)
        {
            ColPair pColPair = ColPairMan.GetActiveColPair();
            pColPair.SetCollision(b, this);
            pColPair.NotifyListeners();
        }

        public override void VisitCrab(Crab c)
        {
            ColPair pColPair = ColPairMan.GetActiveColPair();
            pColPair.SetCollision(c, this);
            pColPair.NotifyListeners();
        }

        public override void VisitOctopus(Octopus o)
        {
            ColPair pColPair = ColPairMan.GetActiveColPair();
            pColPair.SetCollision(o, this);
            pColPair.NotifyListeners();
        }

        public override void VisitSquid(Squid s)
        {
            ColPair pColPair = ColPairMan.GetActiveColPair();
            pColPair.SetCollision(s, this);
            pColPair.NotifyListeners();
        }

        public override void Accept(ColVisitor other)
        {
            // Important: at this point we have an Bomb
            // Call the appropriate collision reaction
            other.VisitShip(this);
        }

        public void MoveRight()
        {
            this.movementState.MoveRight(this);
        }

        public void MoveLeft()
        {
            this.movementState.MoveLeft(this);
        }

        public void ShootMissile()
        {
            this.missileState.ShootMissile(this);
        }

        public void SetMissileState(ShipMan.State inState)
        {
            this.missileState = ShipMan.GetState(inState);
        }

        public ShipState GetMissileState()
        {
            return this.missileState;
        }

        public void SetMovementState(ShipMan.State inState)
        {
            this.movementState = ShipMan.GetState(inState);
        }

        public void ShipDestroyed()
        {
            this.pShipMan.ShipDestroyed();
        }

        public override void Remove()
        {
            // Keenan(delete.E)
            // Since the Root object is being drawn
            // 1st set its size to zero
            //this.poColObj.poColRect.Set(0, 0, 0, 0);
            //base.Update();
            // Now remove it
            base.Remove();

            // Update the parent (missile root)
            GameObject pParent = (GameObject)this.pParent;
            pParent.Update();         
        }

        // Data: --------------------
        public float shipSpeed;
        private ShipState missileState;
        private ShipState movementState;
        private ShipMan pShipMan;
    }
}
