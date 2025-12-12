add_executable(day12)
target_sources(day12
    PRIVATE
    day12.cpp)
target_link_libraries(day12 read_file)
target_include_directories(day12 PRIVATE ../utils)
