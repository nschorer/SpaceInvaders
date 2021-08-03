using System.Diagnostics;

namespace SpaceInvaders
{
    //---------------------------------------------------------------------------------------------------------
    // Design Notes:
    //
    //  Only "new" happens in the default constructor Texture()
    //
    //  Managers - create a pool of them...
    //  Add - Takes one and reuses it by using Set() 
    //
    //---------------------------------------------------------------------------------------------------------
    public class Texture : DLink
    {
        //---------------------------------------------------------------------------------------------------------
        // Enum
        //---------------------------------------------------------------------------------------------------------
        public enum Name
        {
            Default, // HotPink
            Aliens,
            Stitch,
            Birds,
            PacMan,

            SpaceInvaders,
            Consolas20pt,
            Consolas36pt,

            NullObject,
            Uninitialized
        }

        //---------------------------------------------------------------------------------------------------------
        // Constructors
        //---------------------------------------------------------------------------------------------------------
        public Texture()
            : base()   // <--- Delegate (kick the can)
        {
            // Sets everything to known state
            this.privClear();
        }

        //---------------------------------------------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------------------------------------------
        public void Set(Name name, string pTextureName)
        {
            // Copy the data over
            this.name = name;

            Debug.Assert(pTextureName != null);
            Debug.Assert(this.poAzulTexture != null);

            //  Here is a Texture Swap
            //
            //  Replace the existing texture
            //     Manage Language is doing some work here....
            //     Since we are replacing the "HotPink" texture, its removing its reference
            //     A new allocation is replacing the old "HotPink"
            //     Now the old "HotPink" is marked for garabage collection....but its a static (yeah) no GC.
            //     But if it Set() is called on a User defined texture multiple times... GC is envoked
            //
            //  Not super happy... but this only happens in setup.

            if (System.IO.File.Exists(pTextureName))
            {
                // Replace the Default with the new one
                this.poAzulTexture = new Azul.Texture(pTextureName, Azul.Texture_Filter.NEAREST, Azul.Texture_Filter.NEAREST);
            }
            Debug.Assert(this.poAzulTexture != null);
        }
        private void privClear()
        {
            // NOTE:
            //        Do not clear the poAzulTexture it is created once in Default then replaced in Set
            Debug.Assert(Texture.psDefaultAzulTexture != null);

            // Set texture to safety texture Pink
            this.poAzulTexture = psDefaultAzulTexture;
            Debug.Assert(this.poAzulTexture != null);

            this.name = Name.Default;
        }

        public Name GetName()
        {
            return name;
        }
        public void Wash()
        {
            this.privClear();
        }
        public Azul.Texture GetAzulTexture()
        {
            Debug.Assert(this.poAzulTexture != null);
            return this.poAzulTexture;
        }
        public void Dump()
        {

            // Dump - Print contents to the debug output window
            //        Using HASH code as its unique identifier 
            Debug.WriteLine("   Name: {0} ({1})", this.name, this.GetHashCode());

            if (this.poAzulTexture != null)
            {
                Debug.WriteLine("   Texture: {0} ", this.poAzulTexture.GetHashCode());
            }
            else
            {
                Debug.WriteLine("   Texture: null ");
            }


            if (this.pNext == null)
            {
                Debug.WriteLine("      next: null");
            }
            else
            {
                Texture pTmp = (Texture)this.pNext;
                Debug.WriteLine("      next: {0} ({1})", pTmp.name, pTmp.GetHashCode());
            }

            if (this.pPrev == null)
            {
                Debug.WriteLine("      prev: null");
            }
            else
            {
                Texture pTmp = (Texture)this.pPrev;
                Debug.WriteLine("      prev: {0} ({1})", pTmp.name, pTmp.GetHashCode());
            }
        }

        //---------------------------------------------------------------------------------------------------------
        // Data
        //---------------------------------------------------------------------------------------------------------
        public Name name;
        private Azul.Texture poAzulTexture;

        //---------------------------------------------------------------------------------------------------------
        // Static Data
        //---------------------------------------------------------------------------------------------------------
        static private readonly Azul.Texture psDefaultAzulTexture = new Azul.Texture("HotPink.tga");

    }
}

// End of file
