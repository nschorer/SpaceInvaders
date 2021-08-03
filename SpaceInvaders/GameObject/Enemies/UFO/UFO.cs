using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class UFO : EnemyCategory
    {
        public UFO(GameObject.Name name, GameSprite.Name spriteName, float posX, float posY, bool goRight)
            : base(name, spriteName)
        {

            this.Set(posX, posY, goRight);
            //UFOMan.PlayHighSound();
        }

        // Data: ---------------
        ~UFO()
        {

        }

        public void Set(float posX, float posY, bool goRight)
        {
            this.x = posX;
            this.y = posY;

            ufoSpeed = 3f;
            if (!goRight) ufoSpeed *= -1;
        }

        public override void VisitMissile(Missile m)
        {
            ColPair pColPair = ColPairMan.GetActiveColPair();
            pColPair.SetCollision(m, this);
            pColPair.NotifyListeners();
        }

        public override void Accept(ColVisitor other)
        {
            other.VisitUFO(this);
        }

        public override void Update()
        {
            //UFOMan.PlayHighSound();
            base.Update();
            this.x += ufoSpeed;
        }

        public override void Remove()
        {
            // Keenan(delete.E)
            // Since the Root object is being drawn
            // 1st set its size to zero
            UFOMan.StopHighSound();

            // Now remove it
            base.Remove();

            // Update the parent (missile root)
            GameObject pParent = (GameObject)this.pParent;
            pParent.Update();

            // Don't start the timer until we have removed the old one
            UFOMan.SpawnNewUFO();
        }

        public override int GetPointValue()
        {
            return 100;
        }

        private float ufoSpeed;
    }
}
