using Confluent.Kafka;
using Core.tools;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Kafka
{
    public class Consumer<TKey, TValue>
    {

        private readonly bool _commitOnConsume;
        private readonly ConsumerConfig _consumerConfig;

        public delegate Task MessageConsumedDelegate(Message<TKey, TValue> message);

        public event MessageConsumedDelegate OnMessageConsumed;

        public Consumer(KafkaOprtions options, bool _commitOnConsume = false)
        {
            this._commitOnConsume = _commitOnConsume;
            _consumerConfig = new ConsumerConfig(options.Configuration);
        }

        public Task Consume(string topic, CancellationToken token) {

            var task = Task.Run(async () =>
            {
                var consumer = new ConsumerBuilder<TKey, TValue>(_consumerConfig)
                                .SetValueDeserializer(new JsonDeserializer<TValue>())
                                .Build();

                consumer.Subscribe(topic);

                while (!token.IsCancellationRequested)
                {
                    ConsumeResult<TKey, TValue> consumResult = consumer.Consume(TimeSpan.FromMilliseconds(50));

                    if (consumResult != null && OnMessageConsumed != null)
                    {
                        await OnMessageConsumed.Invoke(consumResult.Message);

                        if (!_commitOnConsume)
                        {
                            consumer.Commit(consumResult);
                        }
                    }
                }
            }, token);

            return task;
        }
    }
}
