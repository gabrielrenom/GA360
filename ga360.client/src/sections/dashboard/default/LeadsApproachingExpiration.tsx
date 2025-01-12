import React, { useContext, useEffect, useMemo, useState } from 'react';
import {
  ColumnDef,
  flexRender,
  getCoreRowModel,
  getPaginationRowModel,
  getSortedRowModel,
  SortingState,
  useReactTable
} from '@tanstack/react-table';
import Box from '@mui/material/Box';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import Divider from '@mui/material/Divider';
import Stack from '@mui/material/Stack';
import { Theme, Typography, useMediaQuery } from '@mui/material';
import MainCard from 'components/MainCard';
import Dot from 'components/@extended/Dot';
import ScrollX from 'components/ScrollX';
import { CSVExport, HeaderSort, SelectColumnSorting, TablePagination } from 'components/third-party/react-table';
import { getAllLeadsApproachingExpiration, LeadsApproachingExpirationModel } from 'api/customer';
import DuendeContext from 'contexts/DuendeContext';
import { ColorProps } from 'types/extended';

interface OrderStatusProps {
  status: string;
}

function OrderStatus({ status }: OrderStatusProps) {
  let color: ColorProps;
  let title: string;

  switch (status) {
    case 'Green':
      color = 'success';
      title = 'Green';
      break;
    case 'Amber':
      color = 'warning';
      title = 'Amber';
      break;
    case 'Red':
      color = 'error';
      title = 'Red';
      break;
    default:
      color = 'primary';
      title = 'Unknown';
  }

  return (
    <Stack direction="row" spacing={1} alignItems="center">
      <Dot color={color} />
      <Typography>{title}</Typography>
    </Stack>
  );
}

export default function LeadsApproachingExpiration() {
  const [leads, setLeads] = useState<LeadsApproachingExpirationModel[]>([]);
  const { user } = useContext(DuendeContext);
  const [sorting, setSorting] = useState<SortingState>([]);

  useEffect(() => {
    async function fetchData() {
      try {
        const data = await getAllLeadsApproachingExpiration(user.trainingCentreId);
        setLeads(data);
      } catch (error) {
        console.error('Failed to fetch leads:', error);
      }
    }

    fetchData();
  }, [user.trainingCentreId]);

  const columns = useMemo<ColumnDef<LeadsApproachingExpirationModel>[]>(
    () => [
      {
        accessorKey: 'name',
        header: 'Name'
      },
      {
        accessorKey: 'dateAdded',
        header: 'Date Added',
        cell: info => {
          const date = new Date(info.getValue() as string);
          return date.toLocaleDateString('en-GB'); // Format as DD/MM/YYYY
        }
      },
      {
        accessorKey: 'expiryDate',
        header: 'Expiry Date',
        cell: info => {
          const date = new Date(info.getValue() as string);
          return date.toLocaleDateString('en-GB'); // Format as DD/MM/YYYY
        }
      },
      {
        accessorKey: 'status',
        header: 'Status',
        cell: info => <OrderStatus status={info.getValue() as string} />
      }
    ],
    []
  );

  const table = useReactTable({
    data: leads,
    columns,
    state: { sorting },
    onSortingChange: setSorting,
    getCoreRowModel: getCoreRowModel(),
    getSortedRowModel: getSortedRowModel(),
    getPaginationRowModel: getPaginationRowModel(),
    initialState: { pagination: { pageIndex: 0, pageSize: 5 } }
  });

  const matchDownSM = useMediaQuery((theme: Theme) => theme.breakpoints.down('sm'));

  const headers = table.getAllColumns().map((column) => ({
    label: typeof column.columnDef.header === 'string' ? column.columnDef.header : '#',
    key: column.columnDef.accessorKey as string
  }));

  return (
    <MainCard
      title={matchDownSM ? 'Sorting' : 'Leads Approaching Expiration'}
      content={false}
      secondary={
        <Stack direction="row" alignItems="center" spacing={{ xs: 1, sm: 2 }}>
          <SelectColumnSorting {...{ getState: table.getState, getAllColumns: table.getAllColumns, setSorting }} />
          <CSVExport {...{ data: leads, headers, filename: 'leads_approaching_expiration.csv' }} />
        </Stack>
      }
    >
      <ScrollX>
        <TableContainer component={Paper}>
          <Table>
            <TableHead>
              {table.getHeaderGroups().map(headerGroup => (
                <TableRow key={headerGroup.id}>
                  {headerGroup.headers.map(header => (
                    <TableCell
                      key={header.id}
                      onClick={header.column.getToggleSortingHandler()}
                      className={header.column.getCanSort() ? 'cursor-pointer prevent-select' : ''}
                    >
                      {header.isPlaceholder ? null : (
                        <Stack direction="row" spacing={1} alignItems="center">
                          <Box>{flexRender(header.column.columnDef.header, header.getContext())}</Box>
                          {header.column.getCanSort() && <HeaderSort column={header.column} />}
                        </Stack>
                      )}
                    </TableCell>
                  ))}
                </TableRow>
              ))}
            </TableHead>
            <TableBody>
              {table.getRowModel().rows.map(row => (
                <TableRow key={row.id}>
                  {row.getVisibleCells().map(cell => (
                    <TableCell key={cell.id}>
                      {flexRender(cell.column.columnDef.cell, cell.getContext())}
                    </TableCell>
                  ))}
                </TableRow>
              ))}
            </TableBody>
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
