using System.Collections.Generic;
using System.Xml.Serialization;

namespace JiraWorklogsApp.Common.Models.Tempo
{
    [XmlRoot(ElementName="worklogs")]
	public class Worklogs 
    {
		[XmlElement(ElementName="worklog")]
		public List<Worklog> Items { get; set; }

		[XmlAttribute(AttributeName="date_from")]
		public string DateFrom { get; set; }

		[XmlAttribute(AttributeName="date_to")]
		public string DateTo { get; set; }

		[XmlAttribute(AttributeName="number_of_worklogs")]
		public int NumberOfWorklogs { get; set; }

		[XmlAttribute(AttributeName="format")]
		public string Format { get; set; }

		[XmlAttribute(AttributeName="diffOnly")]
		public bool DiffOnly { get; set; }

		[XmlAttribute(AttributeName="errorsOnly")]
		public bool ErrorsOnly { get; set; }

		[XmlAttribute(AttributeName="validOnly")]
		public bool ValidOnly { get; set; }

		[XmlAttribute(AttributeName="addDeletedWorklogs")]
		public bool AddDeletedWorklogs { get; set; }

		[XmlAttribute(AttributeName="addBillingInfo")]
		public bool AddBillingInfo { get; set; }

		[XmlAttribute(AttributeName="addIssueSummary")]
		public bool AddIssueSummary { get; set; }

		[XmlAttribute(AttributeName="addIssueDescription")]
		public bool AddIssueDescription { get; set; }

		[XmlAttribute(AttributeName="duration_ms")]
		public int DurationMs { get; set; }

		[XmlAttribute(AttributeName="headerOnly")]
		public bool HeaderOnly { get; set; }

		[XmlAttribute(AttributeName="userName")]
		public string UserName { get; set; }

		[XmlAttribute(AttributeName="addIssueDetails")]
		public bool AddIssueDetails { get; set; }

		[XmlAttribute(AttributeName="addParentIssue")]
		public bool AddParentIssue { get; set; }

		[XmlAttribute(AttributeName="addUserDetails")]
		public bool AddUserDetails { get; set; }

		[XmlAttribute(AttributeName="addWorklogDetails")]
		public bool AddWorklogDetails { get; set; }

		[XmlAttribute(AttributeName="billingKey")]
		public string BillingKey { get; set; }

		[XmlAttribute(AttributeName="issueKey")]
		public string IssueKey { get; set; }

		[XmlAttribute(AttributeName="projectKey")]
		public string ProjectKey { get; set; }

		[XmlAttribute(AttributeName="addApprovalStatus")]
		public bool AddApprovalStatus { get; set; }
	}
}
