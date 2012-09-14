﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XnaGame
{
    public class SpatialComponent: BaseComponent<SpatialComponent>, IEntityComponent
    {

        //IntrusiveListItem<SpatialComponent> _link;
        Vector3 _position;
        Vector3 _rotation;
        Vector3 _scale;
        Matrix _worldMatrix;

        //flag to mark the need to update the matrix. Initialized 
        private bool _matrixIsDirty;

        public SpatialComponent(): base()
        {            
            _position = Vector3.Zero;
            _rotation = Vector3.Zero;
            _scale = Vector3.One;
            _matrixIsDirty = true;

            IntrusiveListItem<SpatialComponent>.AddToTail(this);
        }

        public string Name { get { return "Spatial"; } }

        public void LinkPrev(IEntityComponent comp)
        {
            _link.Prev = (SpatialComponent)comp;            
        }

        public void LinkNext(IEntityComponent comp)
        {
            _link.Next = (SpatialComponent)comp;                
        }

        public Vector3 Position
        {
            get { return _position; }
            set
            {
                if (_position == value)//Only move if really moved
                    return; 
                _position = value;
                _matrixIsDirty = true;                 
            }
        }

        /// <summary>
        /// Rotation of the entity in Euler angles. Note that when setting the
        /// rotation, the world matrix will be auto updated when requested.
        /// </summary>
        public Vector3 Rotation
        {
            get { return _rotation; }
            set
            {
                if (_rotation == value) //Only rotate if really rotated
                    return;
                _rotation = value;
                _matrixIsDirty = true;
            }
        }
        
        public Vector3 Scale
        {
            get { return _scale; }
            set
            {
                if (_scale == value) //Only scale if its different
                    return;
                _scale = value;
                _matrixIsDirty = true;
            }
        }
 

        /// <summary>
        /// World matrix used to correctly position the entity in the world. Note
        /// that if the matrix is outdated, it will be recalculated before being
        /// returned
        /// </summary>
        public Matrix WorldMatrix
        {
            get
            {
                if (_matrixIsDirty)
                    UpdateWorldMatrix();
                return _worldMatrix;
            }
        }
        
        
        /// <summary>
        /// Method to update the world matrix. This should be auto-called when needed.
        /// This creates the final matrix by creating a matrix for scale, rotation and
        /// translation, and multiplying it to get the composed matrix
        /// </summary>
        void UpdateWorldMatrix()
        {
            _worldMatrix = Matrix.CreateScale(Scale) *
                    Matrix.CreateFromYawPitchRoll(Rotation.X,Rotation.Y,Rotation.Z) * 
                    Matrix.CreateTranslation(Position); 
            _matrixIsDirty = false; 
        }
               
    }
}
