using Azure.Identity;
using EssentialLayers.Tenant.Models;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EssentialLayers.Tenant.Helpers
{
	public class GraphHelper
	{
		private readonly GraphServiceClient _graphServiceClient;

		public GraphHelper(AzureAd azureAd)
		{
			ClientSecretCredential clientSecretCredential = new(
				azureAd.TenantId,
				azureAd.ClientId,
				azureAd.ClientSecret,
				new TokenCredentialOptions
				{
					AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
				}
			);

			_graphServiceClient = new GraphServiceClient(clientSecretCredential);
		}

		public async Task<List<string>> GetRolesAsync(string objectId)
		{
			var groups = await _graphServiceClient.Users[objectId].MemberOf.GetAsync();
			var roles = new List<string>();

			return roles;
		}

		public async Task<List<User>> GetUsersAsync(string groupId)
		{
			UserCollectionResponse? members = await _graphServiceClient.Groups[groupId].Members.GraphUser.GetAsync();
			List<User> users = [];

			return users;
		}

		public async Task<List<Group>> GetUserGroupsAsync()
		{
			GroupCollectionResponse? groups = await _graphServiceClient.Groups.GetAsync();

			return groups!.Value!;
		}
	}
}