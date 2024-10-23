import React from 'react';

const JobsTable = () => {
  const jobs = [
    { id: 1, name: 'Oil Change', status: 'In Progress', mechanic: 'John Doe', createdAt: '2024-10-23' },
    { id: 2, name: 'Brake Repair', status: 'Completed', mechanic: 'Jane Smith', createdAt: '2024-10-21' },
    { id: 3, name: 'Engine Check', status: 'Pending', mechanic: 'Alex Johnson', createdAt: '2024-10-20' },
  ];

  return (
    <div className="overflow-x-auto">
      <table className="min-w-full border-collapse border border-gray-300">
        <thead>
          <tr>
            <th className="border border-gray-300 px-4 py-2">#</th>
            <th className="border border-gray-300 px-4 py-2">Name</th>
            <th className="border border-gray-300 px-4 py-2">Status</th>
            <th className="border border-gray-300 px-4 py-2">Mechanic</th>
            <th className="border border-gray-300 px-4 py-2">Created At</th>
          </tr>
        </thead>
        <tbody>
          {jobs.map((job) => (
            <tr key={job.id}>
              <td className="border border-gray-300 px-4 py-2">{job.id}</td>
              <td className="border border-gray-300 px-4 py-2">{job.name}</td>
              <td className="border border-gray-300 px-4 py-2">{job.status}</td>
              <td className="border border-gray-300 px-4 py-2">{job.mechanic}</td>
              <td className="border border-gray-300 px-4 py-2">{job.createdAt}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default JobsTable;
