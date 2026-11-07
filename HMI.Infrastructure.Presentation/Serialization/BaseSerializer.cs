// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Events;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;
using HMI.Common.Instrumentation.ErrorHandling;


namespace HMI.Infrastructure.Presentation.Serialization
{
    /// <summary>
    /// 基础序列化器
    /// </summary>
    public abstract class BaseSerializer : ISerializer
    {
        private SubscriptionToken subscriptionToken;// 反序列化完成订阅令牌
        private readonly IList<Action> finishedActions = new List<Action>();// 延迟操作列表
        //升级转换缓存
        //internal static readonly Dictionary<string, XslCompiledTransform> dictionary_0 = new Dictionary<string, XslCompiledTransform>();

        /// <summary>
        /// 序列化器工厂
        /// </summary>
        public ISerializerFactory SerializerFactory
        {
            get;
            set;
        }
        /// <summary>
        /// 依赖注入容器
        /// </summary>
        public IServiceProvider Container
        {
            get;
            private set;
        }
        /// <summary>
        /// 事件聚合器
        /// </summary>
        public IEventAggregator EventAggregator
        {
            get;
            private set;
        }
        /// <summary>
        /// 基础序列化器
        /// </summary>
        /// <param name="container"></param>
        /// <param name="serializerFactory"></param>
        protected BaseSerializer(IServiceProvider container, ISerializerFactory serializerFactory)
        {
            Contract.Requires<ArgumentNullException>(container != null, "container");
            Contract.Requires<ArgumentNullException>(serializerFactory != null, "serializerFactory");
            SerializerFactory = serializerFactory;
            Container = container;
            EventAggregator = Container.Resolve<IEventAggregator>();
        }
        /// <summary>
        /// 反序列化结束事件，在此执行延迟操作
        /// </summary>
        /// <param name="viewModel"></param>
        public virtual void OnDeserializationFinished(IExplorerNodeViewModel viewModel)
        {
            // 取消订阅反序列化完成事件
            DeserializationFinishedEvent @event = EventAggregator.GetEvent<DeserializationFinishedEvent>();
            if (@event != null)
            {
                @event.Unsubscribe(subscriptionToken);
            }

            while (finishedActions.Count > 0)
            {
                finishedActions[0]();
                finishedActions.RemoveAt(0);
            }
        }
        /// <summary>
        /// 延迟操作，在反序列化完成后执行
        /// </summary>
        /// <param name="deferredAction">延迟操作</param>
        protected void Deferred(Action deferredAction)
        {
            finishedActions.Add(deferredAction);
        }
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public abstract JObject? Serialize(object source);

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="ancestorNode">上级节点</param>
        /// <param name="root"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public object Deserialize(IExplorerNodeViewModel ancestorNode, JObject root, Version version)
        {
            finishedActions.Clear();// 清除延迟操作列表
            subscribeFinishedEvent();// 订阅反序列化完成事件

            //root = this.UpgradeXml(root, version);
            return GetDeserializedObjectFromUpgraded(ancestorNode, root, version);
        }
        /// <summary>
        /// 从升级后的节点获取反序列化对象
        /// </summary>
        /// <param name="ancestorNode"></param>
        /// <param name="root"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected virtual object GetDeserializedObjectFromUpgraded(IExplorerNodeViewModel ancestorNode, JObject root, Version version)
        {
            throw new NotImplementedException();
        }

        void subscribeFinishedEvent()
        {
            DeserializationFinishedEvent @event = this.EventAggregator.GetEvent<DeserializationFinishedEvent>();
            if (@event != null)
            {
                this.subscriptionToken = @event.Subscribe(new Action<IExplorerNodeViewModel>(this.OnDeserializationFinished), true);
            }
        }
    }
}
