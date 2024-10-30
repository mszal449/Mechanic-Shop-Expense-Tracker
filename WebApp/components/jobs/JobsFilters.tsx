'use client'
import React, { Dispatch, SetStateAction, useEffect } from 'react';

interface JobsFiltersProps {
  filters : IJobFilters | null,
  setFilters: Dispatch<SetStateAction<IJobFilters | null>>;
}


const JobsFilters = ({filters, setFilters} : JobsFiltersProps) => {
  const handleInputChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { id, value } = e.target;
    setFilters((prevFilters) => ({
      ...prevFilters,
      [id]: value === '' ? undefined : value,
    }));
  };

  useEffect(() => {
    setFilters(filters);
    console.log(filters);
  }, [filters, setFilters]);
  
  return (
    <div className="flex flex-col gap-4 w-full p-4">
      {/* Job Name Filter */}
      <div className="flex flex-col">
        <label htmlFor="name" className="filters--label">
          Job Name
        </label>
        <input
          type="text"
          id="name"
          className="filters--text-input"
          placeholder="Enter job name"
          value={filters?.name || ''}
          onChange={handleInputChange}
        />
      </div>

      {/* Status Filter */}
      <div className="flex flex-col">
        <label htmlFor="jobStatus" className="filters--label">
          Status
        </label>
        <select
          id="jobStatus"
          className="filters--text-input"
          value={filters?.jobStatus ?? ''}
          onChange={handleInputChange}
        >
          <option value="">All Statuses</option>
          <option value="0">Pending</option>
          <option value="1">In Progress</option>
          <option value="2">Completed</option>
          <option value="3">Cancelled</option>
        </select>
      </div>

      {/* Supervisor Filter */}
      <div className="flex flex-col">
        <label htmlFor="supervisor" className="filters--label">
          Supervisor
        </label>
        <input
          type="text"
          id="supervisor"
          className="filters--text-input"
          placeholder="Enter supervisor name"
          value={filters?.supervisor || ''}
          onChange={handleInputChange}
        />
      </div>

      {/* Vehicle Model Filter */}
      <div className="flex flex-col">
        <label htmlFor="carModel" className="filters--label">
          Vehicle Model
        </label>
        <input
          type="text"
          id="carModel"
          className="filters--text-input"
          placeholder="Enter vehicle model"
          value={filters?.carModel || ''}
          onChange={handleInputChange}
        />
      </div>

      {/* Date Created Filter */}
      <div className="flex flex-col">
        <label htmlFor="createdAt" className="filters--label">
          Date Created
        </label>
        <input
          type="date"
          id="createdAt"
          className="filters--text-input"
          // value={filters?.createdAt || ''}
          // onChange={handleInputChange}
        />
      </div>
    </div>
  );
};

export default JobsFilters;
