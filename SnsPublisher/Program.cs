using Amazon.SimpleNotificationService.Model;
using Amazon.SimpleNotificationService;
using SnsPublisher;
using System.Text.Json;

var customer = new CustomerCreated
{
    Id = Guid.NewGuid(),
    Email = "jane@doe.com",
    FullName = "Jane Doe",
    DateOfBirth = new DateTime(2000, 1, 1),
    GitHubUsername = "janedoe"
};

var snsClient = new AmazonSimpleNotificationServiceClient();

var topicArnResponse = await snsClient.FindTopicAsync("customers");

var publishRequest = new PublishRequest
{
    TopicArn = topicArnResponse.TopicArn,
    Message = JsonSerializer.Serialize(customer),
    MessageAttributes = new Dictionary<string, MessageAttributeValue>
    {
        {
            "MessageType", new MessageAttributeValue
            {
                DataType = "String",
                StringValue = nameof(CustomerCreated)
            }
        }
    }
};

var response  = await snsClient.PublishAsync(publishRequest);