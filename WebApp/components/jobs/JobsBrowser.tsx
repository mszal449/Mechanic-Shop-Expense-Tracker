import React from 'react';
import JobsFilters from './JobsFilters';
import JobsTable from './JobsTable';

const JobsBrowser = () => {
  return (
    <div className="grid grid-cols-12 gap-4">
      <div className="col-span-3">
        <JobsFilters />
      </div>
      <div className="col-span-9 ">
        <JobsTable />
      </div>
    </div>
  );
};

export default JobsBrowser;