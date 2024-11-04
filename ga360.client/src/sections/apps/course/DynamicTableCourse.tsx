// // import React from 'react';

// // export default function DynamicTableCourse () {
// //   return (
// //     <div>
// //       <h1>Hello, World! course</h1>
// //     </div>
// //   );
// // }

// /* eslint-disable @typescript-eslint/no-use-before-define */
// import * as React from 'react';
// import { DataGrid, GridToolbarContainer, GridToolbarExport } from '@mui/x-data-grid';
// import {
//   randomCreatedDate,
//   randomTraderName,
//   randomUpdatedDate,
// } from '@mui/x-data-grid-generator';

// export default function DynamicTableCourse() {
//   const [rows, setRows] = React.useState(initialRows);

//   const handleAddRow = () => {
//     const newRow = {
//       id: rows.length + 1,
//       name: randomTraderName(),
//       age: Math.floor(Math.random() * 100),
//       dateCreated: randomCreatedDate(),
//       lastLogin: randomUpdatedDate(),
//     };
//     setRows([...rows, newRow]);
//   };

//   return (
//     <div style={{ height: 300, width: '100%' }}>
//       <DataGrid
//         rows={rows}
//         columns={columns}
//         components={{
//           Toolbar: CustomToolbar,
//         }}
//       />
//       <button onClick={handleAddRow}>Add Row</button>
//     </div>
//   );
// }

// const CustomToolbar = () => {
//   return (
//     <GridToolbarContainer>
//       <GridToolbarExport />
//     </GridToolbarContainer>
//   );
// };

// const columns = [
//   { field: 'name', headerName: 'Name', width: 180, editable: true },
//   { field: 'age', headerName: 'Age', type: 'number', editable: true },
//   {
//     field: 'dateCreated',
//     headerName: 'Date Created',
//     type: 'date',
//     width: 180,
//     editable: true,
//   },
//   {
//     field: 'lastLogin',
//     headerName: 'Last Login',
//     type: 'dateTime',
//     width: 220,
//     editable: true,
//   },
// ];

// const initialRows = [
//   {
//     id: 1,
//     name: randomTraderName(),
//     age: 25,
//     dateCreated: randomCreatedDate(),
//     lastLogin: randomUpdatedDate(),
//   },
//   {
//     id: 2,
//     name: randomTraderName(),
//     age: 36,
//     dateCreated: randomCreatedDate(),
//     lastLogin: randomUpdatedDate(),
//   },
//   {
//     id: 3,
//     name: randomTraderName(),
//     age: 19,
//     dateCreated: randomCreatedDate(),
//     lastLogin: randomUpdatedDate(),
//   },
//   {
//     id: 4,
//     name: randomTraderName(),
//     age: 28,
//     dateCreated: randomCreatedDate(),
//     lastLogin: randomUpdatedDate(),
//   },
//   {
//     id: 5,
//     name: randomTraderName(),
//     age: 23,
//     dateCreated: randomCreatedDate(),
//     lastLogin: randomUpdatedDate(),
//   },
// ];
