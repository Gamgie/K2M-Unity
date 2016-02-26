using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityOSC;

public class K2MClient : MonoBehaviour
{
    public GameObject bodyPrefab;

    public string localPort = "9090";

    private Dictionary<int, K2MBody> _Bodies = new Dictionary<int, K2MBody>();
    OSCServer server;

    void Start()
    {
        server = new OSCServer(int.Parse(localPort));
        server.PacketReceivedEvent += packetReceived;
    }

    void packetReceived(OSCPacket p)
    {
        OSCMessage m = (OSCMessage)p;
        //Debug.Log("Message received " + m.Address);

        if(m.Address == "/k2m/body/entered")
        {
            addBody((int)m.Data[0]);
        }else if(m.Address == "/k2m/body/left")
        {
            removeBody((int)m.Data[0]);
        }else if(m.Address == "/k2m/body/update")
        {

        }else if(m.Address == "/k2m/joint")
        {
            //Debug.Log(m.Data.Count + " arguments");
            updatejoint((int)m.Data[0], (int)m.Data[1], new Vector3((float)m.Data[2], (float)m.Data[3], (float)m.Data[4]));
        }
    }

    void Update()
    {
        server.Update();
    }

    void addBody(int trackingId)
    {

        _Bodies[trackingId] = GameObject.Instantiate(bodyPrefab).GetComponent<K2MBody>();
        _Bodies[trackingId].transform.parent = transform;        
    }

    void removeBody(int trackingId)
    {
        GameObject.Destroy(_Bodies[trackingId].gameObject);
        _Bodies.Remove(trackingId);
    }

    void updatejoint(int trackingId, int jointIndex, Vector3 pos)
    {
        if (!_Bodies.ContainsKey(trackingId)) addBody(trackingId);

        K2MBody b = _Bodies[trackingId];
        b.updateJoint(jointIndex,pos);
    }

    void OnDestroy()
    {
        server.Close();
    }
}
