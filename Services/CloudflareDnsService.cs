using CloudFlare.Client;
using CloudFlare.Client.Api.Zones.DnsRecord;
using CloudFlare.Client.Enumerators;

namespace pWallet.Services
{
    public class CloudflareDnsService
    {
        private readonly CloudFlareClient _client;
        private readonly string _zoneId;

        public CloudflareDnsService(string apiToken, string zoneId)
        {
            _client = new CloudFlareClient(apiToken);
            _zoneId = zoneId;
        }
        public async Task<string> AddTxtRecord(string name, string content)
        {
	        if (!content.StartsWith("bitcoin:?lno="))
	        {
		        content = "bitcoin:?lno=" + content;
	        }

	        var chunks = SplitIntoChunks(content, 255);

	        var newRecord = new NewDnsRecord
	        {
		        Type = DnsRecordType.Txt,
		        Name = $"{name}.user._bitcoin-payment",
		        Content = string.Join("", chunks),
		        Ttl = 1
	        };

	        var result = await _client.Zones.DnsRecords.AddAsync(_zoneId, newRecord);
	        return result.Result.Id;
        }

        private IEnumerable<string> SplitIntoChunks(string str, int chunkSize)
        {
	        yield return str.Substring(0, Math.Min(chunkSize, str.Length));

	        for (int i = chunkSize; i < str.Length; i += chunkSize)
		        yield return str.Substring(i, Math.Min(chunkSize, str.Length - i));
        }


        public async Task<bool> CheckIfRecordExists(string name)
        {
	        try
	        {
		        var fullName = $"{name}.user._bitcoin-payment.pwallet.app";
		        var records = await _client.Zones.DnsRecords.GetAsync(_zoneId);

		        foreach (var record in records.Result)
		        {
			        Console.WriteLine($"Checking DNS Record: Name={record.Name}");
		        }

		        return records.Result.Any(r => r.Name.Equals(fullName, StringComparison.OrdinalIgnoreCase));
	        }
	        catch (Exception ex)
	        {
		        Console.WriteLine($"Error checking DNS record: {ex.Message}");
		        return false;
	        }
        }

        public async Task<DnsRecord?> GetExistingRecord(string name)
        {
	        var records = await _client.Zones.DnsRecords.GetAsync(_zoneId);

	        var existingRecord = records.Result.FirstOrDefault(r =>
		        r.Type == DnsRecordType.Txt &&
		        r.Name.Equals($"{name}.user._bitcoin-payment.pwallet.app", StringComparison.OrdinalIgnoreCase));
	        return existingRecord;
        }

        public async Task<bool> DeleteDnsRecord(string name, string offerContent)
        {
	        try
	        {
		        var records = await _client.Zones.DnsRecords.GetAsync(_zoneId);

		        foreach (var record in records.Result)
		        {
			        Console.WriteLine($"Found Record: Name={record.Name}, Content={record.Content}");
		        }

		        var recordToDelete = records.Result.FirstOrDefault(r =>
			        r.Type == DnsRecordType.Txt &&
			        r.Name.Equals($"{name}.user._bitcoin-payment.pwallet.app", StringComparison.OrdinalIgnoreCase) &&
			        r.Content.Equals($"bitcoin:?lno={offerContent}", StringComparison.OrdinalIgnoreCase));

		        if (recordToDelete != null)
		        {
			        Console.WriteLine($"Deleting Record: Name={recordToDelete.Name}, Content={recordToDelete.Content}");
			        var result = await _client.Zones.DnsRecords.DeleteAsync(_zoneId, recordToDelete.Id);
			        return result.Success;
		        }
		        else
		        {
			        Console.WriteLine($"No matching DNS record found for deletion: Name={name}, OfferContent={offerContent}");
			        return false;
		        }
	        }
	        catch (Exception ex)
	        {
		        Console.WriteLine($"Error deleting DNS record: {ex.Message}");
		        return false;
	        }
        }

        public async Task<string?> FindRecordNameByContent(string content)
        {
	        var records = await _client.Zones.DnsRecords.GetAsync(_zoneId);
	        var matchingRecord = records.Result.FirstOrDefault(r => 
		        r.Type == DnsRecordType.Txt && 
		        r.Content.Contains(content));
    
	        if (matchingRecord != null)
	        {
		        // Extract the username from the record name
		        var parts = matchingRecord.Name.Split('.');
		        return parts[0];
	        }
    
	        return null;
        }

        public async Task<bool> UpdateTxtRecord(string oldName, string newName, string content)
        {
	        // Delete the old record
	        await DeleteDnsRecord(oldName, content);

	        // Add the new record
	        var recordId = await AddTxtRecord(newName, content);

	        return !string.IsNullOrEmpty(recordId);
        }

        public async Task<string> GetDomainNameAsync()
        {
	        try
	        {
		        var zone = await _client.Zones.GetDetailsAsync(_zoneId);
		        return zone.Result.Name;
	        }
	        catch (Exception ex)
	        {
		        Console.WriteLine($"Error fetching domain name: {ex.Message}");
		        return string.Empty;
	        }
        }

    }
}
