'use client'
import React, { useState } from 'react';
import JobsFilters from './JobsFilters';
import JobsTable from './JobsTable';

const JobsBrowser = () => {
  const [filters, setFilters] = useState<IJobFilters | null>(null);


  return (
    <div className="grid grid-cols-12 gap-4">
      <div className="col-start-2 col-end-4">
        <JobsFilters setFilters={setFilters}/>
      </div>
      <div className="col-start-4 col-end-11">
        <JobsTable filters={filters}/>
      </div>
    </div>
  );
};

export default JobsBrowser;