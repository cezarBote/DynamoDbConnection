using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System.Net;
using System.Text.Json;

namespace CloudAWS.Models.Services
{
    public class CustomerRepository:ICustomerRepository
    {
        private readonly IAmazonDynamoDB _dynamoDb;

    public CustomerRepository(IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }

        public async Task<bool> CreateAsync(CustomerDto customer)
        {
            var customerAsJson = JsonSerializer.Serialize(customer);
            var itemAsDocument = Document.FromJson(customerAsJson);
            var itemAsAttributes = itemAsDocument.ToAttributeMap();

            var createItemRequest = new PutItemRequest
            {
                TableName = "customers",
                Item = itemAsAttributes
            };

            var response = await _dynamoDb.PutItemAsync(createItemRequest);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        public async Task<CustomerDto?> GetAsync(Guid id)
        {
            var request = new GetItemRequest
            {
                TableName = "customers",
                Key = new Dictionary<string, AttributeValue>
            {
                { "pk", new AttributeValue { S = id.ToString() } },
                { "sk", new AttributeValue { S = id.ToString() } }
            }
            };

            var response = await _dynamoDb.GetItemAsync(request);
            if (response.Item.Count == 0)
            {
                return null;
            }

            var itemAsDocument = Document.FromAttributeMap(response.Item);
            return JsonSerializer.Deserialize<CustomerDto>(itemAsDocument.ToJson());
        }

        public async Task<IEnumerable<CustomerDto>> GetAllAsync()
        {
            var customersDto= new List<CustomerDto>();
            var scanrequest = new ScanRequest
            {
                TableName = "customers",
            };
            var response = await _dynamoDb.ScanAsync(scanrequest);
            foreach (var item in response.Items)
            {
                var id = new AttributeValue();
                var email = new AttributeValue();
                var fullName = new AttributeValue();
                var gitHubUsername = new AttributeValue();
                var dateOfBirth = new AttributeValue();
                var isFetchedId= item.TryGetValue("id", out id);
                var isFetchedEmail = item.TryGetValue("email", out email);
                var isFetchedFullName = item.TryGetValue("fullName", out fullName);
                var isFetchedGitHubUsername = item.TryGetValue("gitHubUsername", out gitHubUsername);
                var isFetchedDateOfBirth = item.TryGetValue("dateOfBirth", out dateOfBirth);
                customersDto.Add(new CustomerDto
                {
                    Id = id.S.ToString(),
                    Email=email.S.ToString(),
                    FullName=fullName.S.ToString(),
                    GitHubUsername= gitHubUsername.S.ToString(),
                    DateOfBirth= DateTime.Parse(dateOfBirth.S.ToString())
                });
            }

            return customersDto;
        }

        public async Task<bool> UpdateAsync(CustomerDto customer)
        {
            var customerAsJson = JsonSerializer.Serialize(customer);
            var itemAsDocument = Document.FromJson(customerAsJson);
            var itemAsAttributes = itemAsDocument.ToAttributeMap();

            var updateItemRequest = new PutItemRequest
            {
                TableName = "customers",
                Item = itemAsAttributes
            };

            var response = await _dynamoDb.PutItemAsync(updateItemRequest);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var deleteItemRequest = new DeleteItemRequest
            {
                TableName = "customers",
                Key = new Dictionary<string, AttributeValue>
            {
                { "pk", new AttributeValue { S = id.ToString() } },
                { "sk", new AttributeValue { S = id.ToString() } }
            }
            };

            var response = await _dynamoDb.DeleteItemAsync(deleteItemRequest);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }
    }
}
