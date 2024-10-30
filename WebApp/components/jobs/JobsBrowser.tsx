'use client'
import React, { useState } from 'react';
import JobsFilters from './JobsFilters';
import JobsTable from './JobsTable';

const JobsBrowser = () => {
  const [filters, setFilters] = useState<IJobFilters | null>(null);


  return (
    <div className="grid grid-cols-12 gap-4">
      <div className="col-span-3">
        <JobsFilters filters={filters} setFilters={setFilters}/>
      </div>
      <div className="col-span-9 ">
        <JobsTable />
      </div>
    </div>
  );
};

export default JobsBrowser;