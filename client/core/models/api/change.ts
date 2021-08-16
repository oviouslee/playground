import { Diff } from '..';

export interface Change {
  id: number,
  isApproved: boolean,

  diffs: Diff[]
}
