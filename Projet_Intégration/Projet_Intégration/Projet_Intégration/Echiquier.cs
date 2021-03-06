﻿using System;
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
        public  Vector3 Origine { get; set; }
        Vector2 Delta { get; set; }
        List<string> Styles { get; set; }
         

        Color CouleurA { get; set; }
        Color CouleurB { get; set; }
        Color CouleurContour { get; set; }

         public List<Cases> ListeCases { get; set; }

        Vector3 position { get; set; }
        float Variation = 0;

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
            ListeCases = new List<Cases>();


            CréerCases();
            
        }

      
        public override void Initialize()
        {
           
          
      
            
            base.Initialize();
        }

       
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
           
            
            base.Update(gameTime);
        }

        public void CréerCases()
        {
            float a = Origine.Z;
            float b = Origine.X;
            for (float x = 0; x < 8; x ++)

            {

                for (float z =0; z < 8; z ++)
                {
                     position = new Vector3(b,Origine.Y,a);
                    if (Variation % 2 == 0)
                    {
                        uneCase = new Cases(this.Game, 1f, Vector3.Zero, position, CouleurA, CouleurContour, new Vector3(Delta.X, Delta.Y, Delta.X), 1f / 60f);

                    }
                    else
                    {
                        uneCase = new Cases(this.Game, 1f, Vector3.Zero, position, CouleurB, CouleurContour, new Vector3(Delta.X,Delta.Y, Delta.X), 1f / 60f);
                    }
                    this.Game.Components.Add(uneCase);
                    
                    ListeCases.Add(uneCase);
                    Variation++;
                    b -= Delta.X;
                } 
                
                b = Origine.X;
                a += Delta.X;

                
               
                Variation += 1;
               
                

            }
            
            

        }
        
        public void ChangerStyle()
        {
           
            


        }
        public void ModifierVisibilitéCases()
        {
            foreach (Cases c in ListeCases)
            {
                c.Visible = !c.Visible;
            }
        }
        public void ActiverCases()
        {
            foreach (Cases c in ListeCases)
            {
                c.Visible = true;
            }
        }
        public void Retirer()
        {
            foreach (Cases c in ListeCases)
            {
                Game.Components.Remove(c);
            }
            Game.Components.Remove(this);
        }
    }
}
