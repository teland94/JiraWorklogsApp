import { Base } from './base.model';

export class JiraConnection extends Base {
  name: string;
  instanceUrl: string;
  userName: string;
  authToken: string;
  tempoAuthToken: string;
  userId: string;
}

export class JiraConnectionVM extends JiraConnection {
  storeToken: boolean;
}

export class JiraConnectionShortInfo extends Base {
  authToken: string;
  tempoAuthToken: string;
  userName: string;
}
