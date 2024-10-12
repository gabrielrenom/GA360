// material-ui
import { Theme } from '@mui/material/styles';
import useMediaQuery from '@mui/material/useMediaQuery';
import Chip from '@mui/material/Chip';
import Divider from '@mui/material/Divider';
import Grid from '@mui/material/Grid';
import Link from '@mui/material/Link';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemSecondaryAction from '@mui/material/ListItemSecondaryAction';
import Stack from '@mui/material/Stack';
import Typography from '@mui/material/Typography';

// third-party
import { PatternFormat } from 'react-number-format';

// project import
import MainCard from 'components/MainCard';
import Avatar from 'components/@extended/Avatar';
import LinearWithLabel from 'components/@extended/progress/LinearWithLabel';

// assets
import AimOutlined from '@ant-design/icons/AimOutlined';
import EnvironmentOutlined from '@ant-design/icons/EnvironmentOutlined';
import MailOutlined from '@ant-design/icons/MailOutlined';
import PhoneOutlined from '@ant-design/icons/PhoneOutlined';

import defaultImages from 'assets/images/users/default.png';
import { TableDataProps } from 'types/table';
import makeData from 'data/react-table';
import { useEffect, useMemo, useState } from 'react';
import { ColumnDef, flexRender, getCoreRowModel, getPaginationRowModel, getSortedRowModel, HeaderGroup, SortingState, useReactTable } from '@tanstack/react-table';
import ReactTable from 'data/react-table';
import { Box, Paper, Table, TableBody, TableCell, TableContainer, TableFooter, TableHead, TableRow } from '@mui/material';
import { CSVExport, HeaderSort, SelectColumnSorting,TablePagination } from 'components/third-party/react-table';
import ScrollX from 'components/ScrollX';

// ==============================|| ACCOUNT PROFILE - BASIC ||============================== //
// types
import { LabelKeyObject } from 'react-csv/lib/core';
import { getCandidate } from 'api/customer';
import { CustomerListExtended } from 'types/customer';
import { getImageUrl, ImagePath } from 'utils/getImageUrl';

interface ReactTableProps {
  columns: ColumnDef<TableDataProps>[];
  data: TableDataProps[];
}

export default function TabCandidateProfile() {
  const [candidate,setCandidate] =  useState<CustomerListExtended>(null);
  const [avatar, setAvatar] = useState<string | undefined>(
    candidate?.avatarImage
      ? candidate.avatarImage
      : defaultImages
  );
  const matchDownMD = useMediaQuery((theme: Theme) => theme.breakpoints.down('md'));
  const data: TableDataProps[] = makeData(1000);
  const columns = useMemo<ColumnDef<TableDataProps>[]>(
    () => [
      {
        header: 'Card/cert name',
        footer: 'Card/cert name',
        accessorKey: 'fullName',
        enableSorting: false
      },
      {
        header: 'Type',
        footer: 'Type',
        accessorKey: 'email'
      },
      {
        header: 'Date/Time',
        footer: 'Date/Time',
        accessorKey: 'age',
        meta: {
          className: 'cell-right'
        }
      },
      {
        header: 'Change',
        footer: 'Change',
        accessorKey: 'role'
      }
    ],
    []
  );

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const response = await getCandidate();
        setAvatar(response.avatarImage);
        setCandidate(response);
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
        title={matchDownSM ? 'Sorting' : 'Cards and Certifications'}
        content={false}
        secondary={
          <Stack direction="row" alignItems="center" spacing={{ xs: 1, sm: 2 }}>
            <SelectColumnSorting {...{ getState: table.getState, getAllColumns: table.getAllColumns, setSorting }} />
            <CSVExport {...{ data, headers, filename: top ? 'pagination-top.csv' : 'pagination-bottom.csv' }}  />
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
            <MainCard>
              <Grid container spacing={3}>
                <Grid item xs={12}>

                  <Stack spacing={2.5} alignItems="center">
                    <Avatar alt="Avatar 1" size="xl" src={defaultImages} />
                    <Stack spacing={0.5} alignItems="center">
                      {
                      candidate!==null?
                      <Typography variant="h5">{candidate.firstName} {candidate.lastName}</Typography>:<></>}
                      <Typography color="secondary">{candidate!=null?candidate.employeeStatus:<></>}</Typography>
                    </Stack>
                  </Stack>
                </Grid>
                
                <Grid item xs={12}>
                  <List component="nav" aria-label="main mailbox folders" sx={{ py: 0, '& .MuiListItem-root': { p: 0, py: 1 } }}>
                    <ListItem>
                      <ListItemIcon>
                        <MailOutlined />
                      </ListItemIcon>
                      <ListItemSecondaryAction>
                        <Typography align="right">{candidate!==null?candidate.email:<></>}</Typography>
                      </ListItemSecondaryAction>
                    </ListItem>
                    <ListItem>
                      <ListItemIcon>
                        <PhoneOutlined />
                      </ListItemIcon>
                      <ListItemSecondaryAction>
                        <Typography align="right">{candidate!==null?candidate.contact:<></>}</Typography>
                      </ListItemSecondaryAction>
                    </ListItem>
                    <ListItem>
                      <ListItemIcon>
                        <AimOutlined />
                      </ListItemIcon>
                      <ListItemSecondaryAction>
                        <Typography align="right">{candidate!==null?candidate.city:<></>}</Typography>
                      </ListItemSecondaryAction>
                    </ListItem>
                  </List>
                </Grid>
              </Grid>
            </MainCard>
          </Grid>
          <Grid item xs={12}>
            <MainCard title="Course Progressions">
              <Grid container spacing={1.25}>
                {candidate!==null && candidate.courses!==null?
                candidate.courses.map((data, index) => (
                  <>
                    <Grid item xs={6}>
                      <Typography color="secondary">{data.name}</Typography>
                    </Grid>
                    <Grid item xs={6}>
                      <LinearWithLabel value={data.progression} />
                    </Grid>
                  </>
                ))
                :<></>}
               
              </Grid>
            </MainCard>
          </Grid>
        </Grid>
      </Grid>
      <Grid item xs={12} sm={7} md={8} xl={9}>
      <ReactTable {...{ data, columns }} />
      </Grid>
    </Grid>
  );
}
