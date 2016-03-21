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
     public abstract class Pieces : Microsoft.Xna.Framework.DrawableGameComponent
    {
         String Nom { get; set; }
         Model Modèle3D { get; set; }
         string Couleur { get; set; }
         Vector3 Position { get; set; }
         int Scale { get; set; }
         char Lettre { get; set; }
         Rectangle HitBox { get; set; }
         
         public bool LogiqueDéplacement(Vector2 déplacement)
         {
             return true;
         }

        public Pieces(Game game,Vector3 positioninitiale,string couleur,string nomModèle)
            : base(game)
        {
            Nom = nomModèle;
            Couleur = couleur;
            Position = positioninitiale;
            this.Game.Components.Add(new ObjetDeBase(this.Game, Couleur + Nom, 0.05f,Vector3.Zero, Position));
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            
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
    }
}
