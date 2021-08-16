import {
  Injectable,
  Optional
} from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { SnackerService } from '..';
import { ServerConfig } from '../../config';
import { Paragraph } from '../../models';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ParagraphService {
  private paragraph = new BehaviorSubject<Paragraph>(null);
  paragraph$ = this.paragraph.asObservable();

  constructor(
    private http: HttpClient,
    private snacker: SnackerService,
    @Optional() private config: ServerConfig
  ) { }

  getParagraph = (id: number): Promise<Paragraph> => new Promise((resolve) => {
    const observer = {
      next: data => {
        resolve(data);
        this.paragraph.next(data);
      },
      error: err => {
        this.snacker.sendErrorMessage(err.error);
        resolve(null);
      }
    };

    this.http.get<Paragraph>(`${this.config.api}paragraph/getParagraph/${id}`)
      .subscribe(observer)
  })

  removeParagraph = (p: Paragraph): Promise<boolean> => new Promise((resolve) => {
    const observer = {
      next: () => {
        this.snacker.sendSuccessMessage(`Paragraph:, ${p.id} is permanently removed`);
        resolve(true);
      },
      error: err => {
        this.snacker.sendErrorMessage(err.error);
        resolve(false);
      }
    };

    this.http.post(`${this.config.api}paragraph/removeParagraph`, p)
      .subscribe(observer)
  })

  addParagraph = (p: Paragraph): Promise<boolean> => new Promise((resolve) => {
    const observer = {
      next: data => {
        this.snacker.sendSuccessMessage(`Paragraph: ${p.id} is successfully created`);
        resolve(data);
      },
      error: err => {
        this.snacker.sendErrorMessage(err.error);
        resolve(false);
      }
    };

    this.http.post(`${this.config.api}paragraph/addParagraph`, p)
      .subscribe(observer)
  })

  updateParagraph = (p: Paragraph): Promise<boolean> => new Promise((resolve) => {
    const observer = {
      next: data => {
        this.snacker.sendSuccessMessage(`Paragraph: ${p.id} has successfully updated`);
        resolve(data);
      },
      error: err => {
        this.snacker.sendErrorMessage(err.error);
        resolve(false);
      }
    };

    this.http.post(`${this.config.api}paragraph/updateParagraph`, p)
      .subscribe(observer)
  })
}
