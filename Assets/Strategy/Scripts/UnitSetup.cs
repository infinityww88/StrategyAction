using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectDawn.Navigation.Hybrid;
using ProjectDawn.Navigation;
using Sirenix.OdinInspector;

public class UnitSetup : MonoBehaviour
{
	public Strategy.UnitConfig config;
	
	[Button]
	void Test(float speed) {
		var agent = GetComponent<AgentAuthoring>();
		var l = agent.EntityLocomotion;
		l.Speed = speed;
		agent.EntityLocomotion = l;
	}
	
    // Start is called before the first frame update
	void Start()
    {
	    /*
	    var agent = GetComponent<AgentAuthoring>();
	    var l = agent.EntityLocomotion;
	    
	    l.Speed = config.speed;
	    l.Acceleration = config.acceleration;
	    l.AngularSpeed = config.angularSpeed;
	    l.AutoBreaking = config.autoBreaking;
	    l.StoppingDistance = config.stopDistance;
	    
	    agent.EntityLocomotion = l;
	    
	    var agentCollider = GetComponent<AgentColliderAuthoring>();
	    var c = agentCollider.EntityCollider;
	    c.Layers = config.agentLayers;
	    agentCollider.EntityCollider = c;
	    
	    var agentSonarAvoid = GetComponent<AgentAvoidAuthoring>();
	    var s = agentSonarAvoid.EntityAvoid;
	    s.Radius = config.sonarAvoidRadius;
	    s.Layers = config.sonarLayers;
	    s.BlockedStop = config.sonarBlockedStop;
	    s.Angle = config.sonarAngle;
	    s.MaxAngle = config.sonarMaxAngle;
	    agentSonarAvoid.EntityAvoid = s;
	    
	    var agentCylinder = GetComponent<AgentCylinderShapeAuthoring>();
	    var shape = agentCylinder.EntityShape;
	    shape.Radius = config.agentRadius;
	    shape.Height = config.agentHeight;
	    agentCylinder.EntityShape = shape;
	    
	    var agentSeparation = GetComponent<AgentSeparationAuthoring>();
	    var sep = agentSeparation.EntitySeparation;
	    sep.Radius = config.agentSeparationRadius;
	    sep.Layers = config.agentSeparationLayers;
	    agentSeparation.EntitySeparation = sep;
	    
	    var agentSmartStop = GetComponent<AgentSmartStopAuthoring>();
	    var stop = agentSmartStop.EntitySmartStop;
	    var g = stop.GiveUpStop;
	    g.FatigueSpeed = config.smartStopFatigueSpeed;
	    g.RecoverySpeed = config.smartStopRecoverySpeed;
	    stop.GiveUpStop = g;
	    agentSmartStop.EntitySmartStop = stop;
	    */
    }
}
