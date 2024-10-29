import React from 'react';

const JobsFilters = () => {
  return (
    <div className="flex flex-col gap-4 w-full p-4">
      <div className='flex flex-col'>
        <label htmlFor="name" className="filters--label">
          Job Name
        </label>
        <input
          type="text"
          id="name"
          className="filters--text-input"
          placeholder="Enter job name"
        />
      </div>

      <div className='flex flex-col'>
        <label htmlFor="status" className="block filters--label">
          Status
        </label>
        <input
          type="text"
          id="status"
          className="filters--text-input"
          placeholder="Enter job status"
        />
      </div>

      <div className='flex flex-col'>
        <label htmlFor="foreman" className="block filters--label">
          Foreman
        </label>
        <input
          type="text"
          id="foreman"
          className="filters--text-input"
          placeholder="Enter foreman name"
        />
      </div>

      <div className='flex flex-col'>
        <label htmlFor="vehicleModel" className="block filters--label">
          Vehicle Model
        </label>
        <input
          type="text"
          id="vehicleModel"
          className="filters--text-input"
          placeholder="Enter vehicle model"
        />
      </div>

      <div className='flex flex-col'>
        <label htmlFor="createdAt" className="block filters--label">
          Date Created
        </label>
        <input
          type="date"
          id="createdAt"
          className="filters--text-input"
        />
      </div>
    </div>
  );
};

export default JobsFilters;
