add_executable(day4)
target_sources(day4
    PRIVATE
    day4.cpp)
target_link_libraries(day4 read_file grid)
target_include_directories(day4 PRIVATE ../utils)
