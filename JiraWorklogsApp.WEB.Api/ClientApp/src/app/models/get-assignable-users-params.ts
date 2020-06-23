import { JiraConnectionShortInfo } from './jira-connection.model';

export class GetAssignableUsersParams {
  constructor(
    public projectKey: string,
    public jiraConnection: JiraConnectionShortInfo) { }
}
