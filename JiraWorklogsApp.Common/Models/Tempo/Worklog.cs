using System;
using System.Xml.Serialization;

namespace JiraWorklogsApp.Common.Models.Tempo
{
    [XmlRoot(ElementName="worklog")]
	public class Worklog 
    {
		[XmlElement(ElementName="worklog_id")]
		public int WorklogId { get; set; }

		[XmlElement(ElementName="jira_worklog_id")]
		public int JiraWorklogId { get; set; }

		[XmlElement(ElementName="tempo_worklog_id")]
		public int TempoWorklogId { get; set; }

		[XmlElement(ElementName="issue_id")]
		public int IssueId { get; set; }

		[XmlElement(ElementName="issue_key")]
		public string IssueKey { get; set; }

		[XmlElement(ElementName="hours")]
		public decimal Hours { get; set; }

		[XmlElement(ElementName="billed_hours")]
		public decimal BilledHours { get; set; }

		[XmlElement(ElementName="work_date")]
		public DateTime WorkDate { get; set; }

		[XmlElement(ElementName="work_date_time")]
		public string WorkDateTime { get; set; }

		[XmlElement(ElementName="username")]
		public string Username { get; set; }

		[XmlElement(ElementName="staff_id")]
		public object StaffId { get; set; }

		[XmlElement(ElementName="billing_key")]
		public object BillingKey { get; set; }

		[XmlElement(ElementName="billing_attributes")]
		public object BillingAttributes { get; set; }

		[XmlElement(ElementName="activity_id")]
		public object ActivityId { get; set; }

		[XmlElement(ElementName="activity_name")]
		public object ActivityName { get; set; }

		[XmlElement(ElementName="work_description")]
		public string WorkDescription { get; set; }

		[XmlElement(ElementName="parent_key")]
		public object ParentKey { get; set; }

		[XmlElement(ElementName="reporter")]
		public string Reporter { get; set; }

		[XmlElement(ElementName="external_id")]
		public object ExternalId { get; set; }

		[XmlElement(ElementName="external_tstamp")]
		public object ExternalTstamp { get; set; }

		[XmlElement(ElementName="external_hours")]
		public decimal ExternalHours { get; set; }

		[XmlElement(ElementName="external_result")]
		public object ExternalResult { get; set; }

		[XmlElement(ElementName="hash_value")]
		public string HashValue { get; set; }

        [XmlElement(ElementName="issue_summary")]
        public string IssueSummary { get; set; }
	}
}
