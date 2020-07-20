//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: This object's rigidbody goes to sleep when created
//
//=============================================================================

using UnityEngine;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
	//-------------------------------------------------------------------------
	public class SleepOnAwake : MonoBehaviour
	{
		//-------------------------------------------------
		void Awake()
		{
			Rigidbody rigidbody = GetComponent<Rigidbody>();
			if ( rigidbody != null )
			{
                rigidbody.velocity = Vector3.zero;
				rigidbody.Sleep();
			}
		}

        private void FixedUpdate()
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            if (rigidbody)
            {
                if (rigidbody.IsSleeping())
                {
                    Destroy(this);
                }
                else
                {
                    rigidbody.velocity = Vector3.zero;
                    rigidbody.Sleep();
                }
            }
        }
    }
}
