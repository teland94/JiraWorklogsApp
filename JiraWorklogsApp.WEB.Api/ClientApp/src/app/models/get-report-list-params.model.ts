import { JiraConnectionShortInfo } from './jira-connection.model';
import { DateRange } from './date-range.model';

export class GetReportListParams {
    constructor(
        public dateRange: DateRange,
        public projectKey: string,
        public jiraConnection: JiraConnectionShortInfo,
        public userName?: string) { }
  timezoneOffset: number;
}
