using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace AtelierXNA
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Echiquier : Microsoft.Xna.Framework.GameComponent
    {
        const int NB_CASES = 64;
        Vector2 Dimensions { get; set; }
        Vector3 Origine { get; set; }
        Vector2 Delta { get; set; }
        List<string> Styles { get; set; }
        List<Cases> ListeCases { get; set; }

        Color CouleurA { get; set; }
        Color CouleurB { get; set; }
        Color CouleurContour { get; set; }
        int Variation = 0;

        Cases uneCase { get; set; }
        public Echiquier(Game game,Vector3 origine,Vector2 dimensions,Color couleurA, Color couleurB ,Color couleurContour)
            : base(game)
        {
            Origine = origine;
            Dimensions = dimensions;
            CouleurA = couleurA;
            CouleurB = couleurB;
            CouleurContour = couleurContour;
            Delta = new Vector2(Dimensions.X / 8, Dimensions.Y);
            

            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            ListeCases = new List<Cases>();
            CréerCases();
            // TODO: Add your initialization code here
            
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public void CréerCases()
        {
            for (float x = Origine.X; x < Origine.X + Dimensions.X; x += Delta.X)
            {
                for (float y = Origine.Y; y < Origine.Y+ Dimensions.X; y += Delta.X)
                {
                    Vector3 position = new Vector3(x,Origine.Y, y);
                    if (Variation % 2 == 0)
                    {
                        uneCase = new Cases(this.Game, 1f, Vector3.Zero, position, CouleurA, Color.WhiteSmoke, new Vector3(2, 0.3f, 2), 1f / 60f);

                    }
                    else
                    {
                        uneCase = new Cases(this.Game, 1f, Vector3.Zero, position, CouleurB, Color.WhiteSmoke, new Vector3(2, 0.3f, 2), 1f / 60f);
                    }
                    this.Game.Components.Add(uneCase);
                    ListeCases.Add(uneCase);
                    Variation++;
                }
                Variation += 1;
                

            }
            

        }
        
        public void ChangerStyle()
        {


        }
    }
}
