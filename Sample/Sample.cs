#if UNITY_EDITOR

namespace Hieki.Pubsub.Sample
{
    internal interface IScene
    {
        public void OnLoadScene();
    }

    internal class SceneA : IScene
    {
        ISubscriber subscriber { get; set; }

        public void OnLoadScene()
        {
            //subscribe with topic
            subscriber.Subscribe<LoadScenePayLoadMessage>(Topic.FromMessage<LoadScenePayLoadMessage>(), static (payload) =>
            {
                UnityEngine.Debug.Log($"state: {payload.state}, value: {payload.value}");
            });
        }
    }


    internal class SceneB : IScene
    {
        IPublisher publisher { get; set; }

        Topic loadTopic = Topic.FromMessage<LoadScenePayLoadMessage>();      //topic to publish

        public void OnLoadScene()
        {
            //create a message and publish to subscribers who are already subscribed to this event.
            LoadScenePayLoadMessage payload = new LoadScenePayLoadMessage(999, "Success");
            publisher.Publish(loadTopic, payload);
        }

    }

    internal class SceneManager
    {
        SceneA sceneA;
        SceneB sceneB;

        public void Awake()
        {
            sceneA = new SceneA();
            sceneB = new SceneB();

            sceneA.OnLoadScene();   //sceneA subscribe to LoadSceneB event
            sceneB.OnLoadScene();   //sceneB load, publish the LoadSceneB event.
        }
    }

    internal readonly struct LoadScenePayLoadMessage : IMessage  //sample message
    {
        public readonly float value;
        public readonly string state;

        public LoadScenePayLoadMessage(float value, string state)
        {
            this.value = value;
            this.state = state;
        }
    }
}
#endif
