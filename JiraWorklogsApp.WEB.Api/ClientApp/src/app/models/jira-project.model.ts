import { JiraConnection, JiraConnectionShortInfo } from './jira-connection.model';
export class JiraProject {
  id: number;
  key: string;
  name: string;

  jiraConnection: JiraConnectionShortInfo;
}
