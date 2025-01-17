// material-ui
import { Theme } from '@mui/material/styles';
import useMediaQuery from '@mui/material/useMediaQuery';
import Divider from '@mui/material/Divider';
import Grid from '@mui/material/Grid';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemSecondaryAction from '@mui/material/ListItemSecondaryAction';
import Stack from '@mui/material/Stack';
import Typography from '@mui/material/Typography';

// project import
import MainCard from 'components/MainCard';
import Avatar from 'components/@extended/Avatar';
import LinearWithLabel from 'components/@extended/progress/LinearWithLabel';

// assets
import AimOutlined from '@ant-design/icons/AimOutlined';
import MailOutlined from '@ant-design/icons/MailOutlined';
import PhoneOutlined from '@ant-design/icons/PhoneOutlined';

import defaultImages from 'assets/images/users/default.png';
import { CourseUserViewDataProps, CourseViewDataProps, TableDataProps } from 'types/table';
import { useEffect, useMemo, useState } from 'react';
import { ColumnDef, flexRender, getCoreRowModel, getPaginationRowModel, getSortedRowModel, HeaderGroup, SortingState, useReactTable } from '@tanstack/react-table';
import { Box, Paper, Table, TableBody, TableCell, TableContainer, TableFooter, TableHead, TableRow } from '@mui/material';
import { CSVExport, HeaderSort, SelectColumnSorting,TablePagination } from 'components/third-party/react-table';
import ScrollX from 'components/ScrollX';

// ==============================|| ACCOUNT PROFILE - BASIC ||============================== //
// types
import { LabelKeyObject } from 'react-csv/lib/core';
import { getCandidate, getUserById } from 'api/customer';
import { CustomerListExtended } from 'types/customer';
import CandidateProfile from './CandidateCRUDProfile';
import MyQualificationsProfile from './MyQualificationsCRUDProfile';
import { useParams } from 'react-router';
import { getCourseByUserId } from 'api/courseService';

interface ReactTableProps {
  columns: ColumnDef<CourseUserViewDataProps>[];
  data: CourseUserViewDataProps[];
}
export default function TabCRUDCandidateCourses() {
  const { id } = useParams();

  const [candidate,setCandidate] =  useState<CustomerListExtended>(null);
  const [courseData,setCourseData] =  useState<CourseUserViewDataProps[]>([]);

  const [avatar, setAvatar] = useState<string | undefined>(
    candidate?.avatarImage
      ? candidate.avatarImage
      : defaultImages
  );
  const matchDownMD = useMediaQuery((theme: Theme) => theme.breakpoints.down('md'));
  
  const columns = useMemo<ColumnDef<CourseUserViewDataProps>[]>(
    () => [
      {
        header: 'Course Name',
        footer: 'Course Name',
        accessorKey: 'coursename',
        enableSorting: false
      },
      {
        header: 'REG DATE',
        footer: 'REG DATE',
        accessorKey: 'registrationDate',
        meta: {
          className: 'cell-right'
        }
      },
      {
        header: 'EXP DATE',
        footer: 'EXP DATE',
        accessorKey: 'expectedDate'
      },
      {
        header: 'CERT DATE',
        footer: 'CERT DATE',
        accessorKey: 'certificateDate'
      },
      {
        header: 'CERT NUMBER',
        footer: 'CERT NUMBER',
        accessorKey: 'certificateNumber'
      },
      {
        header: 'PRICE',
        footer: 'PRICE',
        accessorKey: 'price'
      }
    ],
    []
  );

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const learner = await getUserById(Number(id));
        setAvatar(learner.avatarImage);
        setCandidate(learner);
        const courses = await getCourseByUserId(Number(id));

        const mappedCourses: CourseUserViewDataProps[] = courses.map((course) => ({
          coursename: course.name,
          registrationDate: course.registrationDate ? new Date(course.registrationDate).toLocaleDateString("en-GB") : null,
          expectedDate: course.expectedDate ? new Date(course.expectedDate).toLocaleDateString("en-GB") : null,
          certificateDate: course.certificateDate ? new Date(course.certificateDate).toLocaleDateString("en-GB") : null,
          certificateNumber: course.certificateNumber,
          price: course.price ?? 0 // Handle optional price
        }));
        
        
        setCourseData(mappedCourses); 
      } catch (error) {
        console.error("Error fetching countries:", error);
      }
    };

    fetchUser();
  }, []);

  function ReactTable({ columns, data }: ReactTableProps) {
    const matchDownSM = useMediaQuery((theme: Theme) => theme.breakpoints.down('sm'));
    const [sorting, setSorting] = useState<SortingState>([
      {
        id: 'age',
        desc: false
      }
    ]);
  
    const table = useReactTable({
      data,
      columns,
      state: {
        sorting
      },
      onSortingChange: setSorting,
      getCoreRowModel: getCoreRowModel(),
      getSortedRowModel: getSortedRowModel(),
      getPaginationRowModel: getPaginationRowModel(),

    });
  
    let headers: LabelKeyObject[] = [];
    table.getAllColumns().map((columns) =>
      headers.push({
        label: typeof columns.columnDef.header === 'string' ? columns.columnDef.header : '#',
        // @ts-ignore
        key: columns.columnDef.accessorKey
      })
    );
  
    return (
      <MainCard
        title={matchDownSM ? 'Sorting' : 'Courses'}
        content={false}
        secondary={
          <Stack direction="row" alignItems="center" spacing={{ xs: 1, sm: 2 }}>
            <SelectColumnSorting {...{ getState: table.getState, getAllColumns: table.getAllColumns, setSorting }} />
            <CSVExport {...{ data, headers, filename: top ? 'courses.csv' : 'courses.csv' }}  />
          </Stack>
        }
      >
        <ScrollX>
          <TableContainer component={Paper}>
            <Table>
              <TableHead>
                {table.getHeaderGroups().map((headerGroup: HeaderGroup<any>) => (
                  <TableRow key={headerGroup.id}>
                    {headerGroup.headers.map((header) => {
                      if (header.column.columnDef.meta !== undefined && header.column.getCanSort()) {
                        Object.assign(header.column.columnDef.meta, {
                          className: header.column.columnDef.meta.className + ' cursor-pointer prevent-select'
                        });
                      }
                      return (
                        <TableCell
                          key={header.id}
                          {...header.column.columnDef.meta}
                          onClick={header.column.getToggleSortingHandler()}
                          {...(header.column.getCanSort() &&
                            header.column.columnDef.meta === undefined && {
                              className: 'cursor-pointer prevent-select'
                            })}
                        >
                          {header.isPlaceholder ? null : (
                            <Stack direction="row" spacing={1} alignItems="center">
                              <Box>{flexRender(header.column.columnDef.header, header.getContext())}</Box>
                              {header.column.getCanSort() && <HeaderSort column={header.column} />}
                            </Stack>
                          )}
                        </TableCell>
                      );
                    })}
                  </TableRow>
                ))}
              </TableHead>
              <TableBody>
                {table.getRowModel().rows.map((row) => (
                  <TableRow key={row.id}>
                    {row.getVisibleCells().map((cell) => (
                      <TableCell key={cell.id} {...cell.column.columnDef.meta}>
                        {flexRender(cell.column.columnDef.cell, cell.getContext())}
                      </TableCell>
                    ))}
                  </TableRow>
                ))}
              </TableBody>
              <TableFooter>
                {table.getFooterGroups().map((footerGroup) => (
                  <TableRow key={footerGroup.id}>
                    {footerGroup.headers.map((footer) => (
                      <TableCell key={footer.id} {...footer.column.columnDef.meta}>
                        {footer.isPlaceholder ? null : flexRender(footer.column.columnDef.header, footer.getContext())}
                      </TableCell>
                    ))}
                  </TableRow>
                ))}
              </TableFooter>
            </Table>
          </TableContainer>

              <Divider />
              <Box sx={{ p: 2 }}>
                <TablePagination
                  {...{
                    setPageSize: table.setPageSize,
                    setPageIndex: table.setPageIndex,
                    getState: table.getState,
                    getPageCount: table.getPageCount
                  }}
                />
              </Box>
            
        
        </ScrollX>
      </MainCard>
    );
  }

  return (
    <Grid container spacing={3}>
      <Grid item xs={12} sm={5} md={4} xl={3}>
        <Grid container spacing={3}>
          <Grid item xs={12}>
          <CandidateProfile candidate={candidate} defaultImages={avatar}></CandidateProfile>
          </Grid>
          <Grid item xs={12}>
            <MyQualificationsProfile userId={Number(id)}/>
          </Grid>
        </Grid>
      </Grid>
      <Grid item xs={12} sm={7} md={8} xl={9}>
      <ReactTable data={courseData} columns={columns} />

      </Grid>
    </Grid>
  );
}
