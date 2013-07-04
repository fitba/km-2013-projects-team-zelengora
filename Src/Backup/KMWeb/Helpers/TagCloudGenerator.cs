using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace KMWeb.Helpers
{
    public class TagCloudGenerator
{
	public string GetTagCloudHTML(Dictionary<string, int> tagNameWithFrequesncies)
	{
		StringBuilder tagCloudString = new StringBuilder("");
		int highestFrequency = tagNameWithFrequesncies.Values.Max();
		int counter =1;
		foreach (string tag in tagNameWithFrequesncies.Keys)
		{
			string tagClass = GetTagClass(tagNameWithFrequesncies[tag], highestFrequency);
            //TODO: need to set proper URL where links should redirect to
            string targetUrl = "../SearchResults.aspx?WordToSearch=" + tag;
            string tagItem = "<a class='" + tagClass + "' href='" + targetUrl + "'>" + tag + "</a>";
            counter++;
            if (counter == 5)
            {
                tagCloudString.Append("<br>");
                counter = 0;
            }
			tagCloudString.Append(tagItem);
		}
 
		tagCloudString.Append("");
		return tagCloudString.ToString();
	}
 
	public string GetTagClass(int tagFrequency, int highestFrequency)
	{
        
		if(tagFrequency == 0 || highestFrequency ==0)
		return "tag0";
 
		var percentageFrequency = (tagFrequency * 100) / highestFrequency;
 
		if (percentageFrequency >= 90)
			return "tag90";
		if (percentageFrequency >= 80)
			return "tag80";
		if (percentageFrequency >= 70)
			return "tag70";
		if (percentageFrequency >= 60)
			return "tag60";
		if (percentageFrequency >= 50)
			return "tag50";
		if (percentageFrequency >= 40)
			return "tag40";
		if (percentageFrequency >= 30)
			return "tag30";
		if (percentageFrequency >= 3)
			return "tag20";
		if (percentageFrequency >= 2)
			return "tag10";
		if (percentageFrequency >= 1)
			return "tag1";

        return "tag0";
    }
    }

}