import { AdalService } from 'adal-angular4';
import { JiraConnection, JiraConnectionShortInfo } from './../models/jira-connection.model';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class JiraConnectionsService {

  private apiUrl = '/api/JiraConnections';

  constructor(private httpClient: HttpClient,
    private adalService: AdalService) { }

  get(): Observable<JiraConnection[]> {
    return this.httpClient.get<JiraConnection[]>(this.apiUrl).map(data => {
      data.forEach(conn => {
        if (!conn.authToken) {
          const connection = this.getLocalConnectionById(conn.id);
          if (connection) {
            conn.authToken = connection.authToken;
            conn.tempoAuthToken = connection.tempoAuthToken;
          }
        }
      });
      return data;
    });
  }

  getShortInfo(): Observable<JiraConnectionShortInfo[]> {
    return this.httpClient.get<JiraConnectionShortInfo[]>(this.apiUrl + '/GetShortInfo').map(data => {
      data.forEach(conn => {
        if (!conn.authToken) {
          const connection = this.getLocalConnectionById(conn.id);
          if (connection) {
            conn.authToken = connection.authToken;
            conn.tempoAuthToken = connection.tempoAuthToken;
          }
        }
      });
      return data;
    });
  }

  getItem(id: number): Observable<JiraConnection> {
    return this.httpClient.get<JiraConnection>(this.apiUrl + '/Get/' + id).map(data => {
      if (!data.authToken) {
        const connection = this.getLocalConnectionById(id);
        if (connection) {
          data.authToken = connection.authToken;
          data.tempoAuthToken = connection.tempoAuthToken;
        }
      }
      return data;
    });
  }

  create(jiraConnection: JiraConnection, storeToken: boolean): Observable<number> {
    let token: string;
    let tempoToken: string;
    if (!storeToken) {
      token = jiraConnection.authToken;
      tempoToken = jiraConnection.tempoAuthToken;
      jiraConnection.authToken = null;
      jiraConnection.tempoAuthToken = null;
    }
    return this.httpClient.post<number>(this.apiUrl, jiraConnection).map(data => {
      if (!storeToken) {
        jiraConnection.id = data;
        jiraConnection.authToken = token;
        jiraConnection.tempoAuthToken = tempoToken;
        this.addOrUpdateLocalToken(jiraConnection);
      }
      return data;
    });
  }

  update(jiraConnection: JiraConnection, storeToken: boolean) {
    if (!storeToken) {
      this.addOrUpdateLocalToken(jiraConnection);
      jiraConnection.authToken = null;
      jiraConnection.tempoAuthToken = null;
    } else {
      this.deleteLocalConnection(jiraConnection.id);
    }
    return this.httpClient.put(this.apiUrl, jiraConnection);
  }

  delete(id: number) {
    return this.httpClient.delete(this.apiUrl + '/' + id).map(() => {
      this.deleteLocalConnection(id);
    });
  }

  test(jiraConnection: JiraConnection) {
    return this.httpClient.post(this.apiUrl + '/Test', jiraConnection);
  }

  getLocalConnectionById(id: number): JiraConnectionShortInfo {
    const jiraConnections = this.getLocalConnections();
    return jiraConnections.find(t => t.id === id);
  }

  getUserLocalConnections(): Array<JiraConnectionShortInfo> {
    const jiraConnections = this.getLocalConnections();
    return jiraConnections.filter(u => u.userName === this.adalService.userInfo.userName);
  }

  private addOrUpdateLocalToken(jiraConnection: JiraConnection) {
    const jiraConnections = this.getLocalConnections();
    const index = this.getIndexByProperty(jiraConnections, 'id', jiraConnection.id);
    if (index > -1) {
      jiraConnections[index].authToken = jiraConnection.authToken;
      jiraConnections[index].tempoAuthToken = jiraConnection.tempoAuthToken;
    } else {
      jiraConnections.push({ id: jiraConnection.id, authToken: jiraConnection.authToken, tempoAuthToken: jiraConnection.tempoAuthToken,
        userName: this.adalService.userInfo.userName });
    }
    if (jiraConnections) {
      localStorage.setItem('jiraConnections', JSON.stringify(jiraConnections));
    }
  }

  private deleteLocalConnection(id: number) {
    let jiraConnections = this.getLocalConnections();
    jiraConnections = jiraConnections.filter(function (obj) {
      return obj['id'] !== id;
    });
    if (jiraConnections) {
      localStorage.setItem('jiraConnections', JSON.stringify(jiraConnections));
    }
  }

  private getLocalConnections(): Array<JiraConnectionShortInfo> {
    return localStorage.getItem('jiraConnections') ? JSON.parse(localStorage.getItem('jiraConnections')) : [];
  }

  private getIndexByProperty(data, key, value) {
    for (let i = 0; i < data.length; i++) {
      if (data[i][key] === value) {
        return i;
      }
    }
    return -1;
  }
}
