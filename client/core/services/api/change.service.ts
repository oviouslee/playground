import {
  Injectable,
  Optional
} from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { SnackerService } from '..';
import { ServerConfig } from '../../config';
import { Change } from '../../models';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ChangeService {
  private change = new BehaviorSubject<Change>(null);
  change$ = this.change.asObservable();

  constructor(
    private http: HttpClient,
    private snacker: SnackerService,
    @Optional() private config: ServerConfig
  ) { }

  getChange = (id: number): Promise<Change> => new Promise((resolve) => {
    const observer = {
      next: data => {
        resolve(data);
        this.change.next(data);
      },
      error: err => {
        this.snacker.sendErrorMessage(err.error);
        resolve(null);
      }
    };

    this.http.get<Change>(`${this.config.api}change/getChange/${id}`)
      .subscribe(observer)
  })

  removeChange = (c: Change): Promise<boolean> => new Promise((resolve) => {
    const observer = {
      next: () => {
        this.snacker.sendSuccessMessage(`Change:, ${c.id} is permanently removed`);
        resolve(true);
      },
      error: err => {
        this.snacker.sendErrorMessage(err.error);
        resolve(false);
      }
    };

    this.http.post(`${this.config.api}change/removeChange`, c)
      .subscribe(observer)
  })

  addChange = (c: Change): Promise<boolean> => new Promise((resolve) => {
    const observer = {
      next: data => {
        this.snacker.sendSuccessMessage(`Change: ${c.id} is successfully created`);
        resolve(data);
      },
      error: err => {
        this.snacker.sendErrorMessage(err.error);
        resolve(false);
      }
    };

    this.http.post(`${this.config.api}change/addChange`, c)
      .subscribe(observer)
  })

  updateChange = (c: Change): Promise<boolean> => new Promise((resolve) => {
    const observer = {
      next: data => {
        this.snacker.sendSuccessMessage(`Change: ${c.id} has successfully updated`);
        resolve(data);
      },
      error: err => {
        this.snacker.sendErrorMessage(err.error);
        resolve(false);
      }
    };

    this.http.post(`${this.config.api}change/updateChange`, c)
      .subscribe(observer)
  })
}
