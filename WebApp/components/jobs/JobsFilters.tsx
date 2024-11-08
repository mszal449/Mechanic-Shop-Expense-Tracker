'use client'
import React, { Dispatch, SetStateAction, useState } from 'react';

interface JobsFiltersProps {
  setFilters: Dispatch<SetStateAction<IJobFilters | null>>;
}


const JobsFilters = ({setFilters} : JobsFiltersProps) => {
  const [localFilters, setLocalFilters] = useState<IJobFilters>({});

  const handleInputChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { id, value } = e.target;
    setLocalFilters((prevFilters) => ({
      ...prevFilters,
      [id]: value === '' ? undefined : value,
    }));
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setFilters(localFilters);
  };

  return (
    <form onSubmit={handleSubmit} className="flex flex-col gap-4 w-full p-4">
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
          value={localFilters?.name || ''}
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
          value={localFilters?.jobStatus ?? ''}
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
          value={localFilters?.supervisor || ''}
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
          value={localFilters?.carModel || ''}
          onChange={handleInputChange}
        />
      </div>
      <div className='flex justify-between'>
        {/* Filter Button */}
        <button
          type="submit"
          className="filters--button"
          >
          Filter
        </button>
        {/* Reset Filters Button */}
        <button 
          onClick={() => setLocalFilters({})}
          className='filters--button'
          >
          Reset
        </button>
      </div>
    </form>
  );
};

export default JobsFilters;
