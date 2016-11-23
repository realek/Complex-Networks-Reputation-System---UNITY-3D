using UnityEngine;
using System.Collections;
using System;

public enum NetworkModel
{
    None,
    Barabasi_Albert,
    ER
}

public class NetworkGenerator<T> {

    private static Network<T> m_storedNetwork;
    private Func<bool> linkCondition;
    private int m_maxLinksOnAdd;
    private const int DEFAULT_MAX_LINKS_ON_ADD = 3;

    public NetworkGenerator()
    {
        m_maxLinksOnAdd = DEFAULT_MAX_LINKS_ON_ADD;
        m_storedNetwork = new Network<T>();
    }

    public NetworkGenerator(int maxLinksOnNodeAdd)
    {
        m_maxLinksOnAdd = maxLinksOnNodeAdd;
        m_storedNetwork = new Network<T>();
    }

    public void LoadNodeLinkCondition(Func<bool> nodeLinkCondition)
    {
        linkCondition = nodeLinkCondition;
    }

    public NetworkGenerator<T> Startup(NetworkModel model)
    {
        switch (model)
        {
            case NetworkModel.Barabasi_Albert:
                {
                    BarabasiAlberModel();
                    break;
                }

            case NetworkModel.ER:
                {
                    ER();
                    break;
                }

        }
        return this;

    }

    private void BarabasiAlberModel()
    {

    }

    private void ER()
    {

    }

    public NetworkGenerator<T> MultipleStepNetwork(int nrOfSteps)
    {
        for (int i = 0; i < nrOfSteps; i++)
            StepNetwork();
        return this;
    }

    public NetworkGenerator<T> StepNetwork()
    {
        return this;
    }

    public static implicit operator Network<T>(NetworkGenerator<T> generator)
    {
        Network<T> ret = m_storedNetwork;
        m_storedNetwork = null;
        return ret;
    }


}
