// material-ui
import { TableCellProps } from '@mui/material/TableCell';

// project import
import { Gender } from 'config';

// types
import { KeyedObject } from './root';

export type ArrangementOrder = 'asc' | 'desc' | undefined;

export type GetComparator = (o: ArrangementOrder, o1: string) => (a: KeyedObject, b: KeyedObject) => number;

export interface EnhancedTableHeadProps extends TableCellProps {
  onSelectAllClick: (e: React.ChangeEvent<HTMLInputElement>) => void;
  order: ArrangementOrder;
  orderBy?: string;
  numSelected: number;
  rowCount: number;
  onRequestSort: (e: React.SyntheticEvent, p: string) => void;
}

export interface EnhancedTableToolbarProps {
  numSelected: number;
}

export type HeadCell = {
  id: string;
  numeric: boolean;
  label: string;
  disablePadding?: string | boolean | undefined;
  align?: 'left' | 'right' | 'inherit' | 'center' | 'justify' | undefined;
};

export type TableDataApiResponse = {
  data: TableDataProps[];
  meta: {
    totalRowCount: number;
  };
};

export type TableDataProps = {
  id: number;
  firstName: string;
  lastName: string;
  fullName: string;
  fatherName: string;
  email: string;
  age: number;
  gender: Gender;
  role: string;
  visits: number;
  progress: number;
  status: string;
  orderStatus: string;
  contact: string;
  country: string;
  address: string;
  about: string;
  avatar: number;
  skills: string[];
  time: string[];

  coursename: string;

};

export type CourseViewDataProps = {
  coursename: string;
  date: string;
  duration: number;
  assesor: string;
  card: string;
  certification: string;
};

export type CourseTrainingViewDataProps = {
  coursename: string;
  date: string;
  duration: string;
  assesor: string;
  card: string;
  certification: string;
  learners: number;
};

export type DocumentViewDataProps = {
  name: string;
  url: string;
}

export type QualificationViewDataProps ={
  name: string;
  regDate: string;
  status: string;
  progress: string;
}

export type QualificationTrainingCentreViewDataProps ={
  qualificationId: number;
  qan: number;
  internalReference: string;
  qualificationName: string;
  awardingBody: string;
  learners: number;
  assessors: number;
  expirationDate: Date;
  status: string;
}

export type CertificationViewDataProps ={
  name: string;
  // id: number;
  charge: string;
  type: string;
  regDate: string;
}